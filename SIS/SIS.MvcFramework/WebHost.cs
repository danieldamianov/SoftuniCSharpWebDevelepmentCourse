using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.HTTP.Sessions;
using SIS.MvcFramework.Attributes.ActionAttributes;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Attributes.SecurityAttributes;
using SIS.MvcFramework.Results;
using SIS.MvcFramework.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.MvcFramework
{
    public class WebHost
    {
        public static void Run(IMvcApplication startUp)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpSessionStorage httpSessionStorage = new HttpSessionStorage();
            DependencyContainer.IServiceProvider serviceProvider = new DependencyContainer.ServiceProvider();

            startUp.ConfigureServices(serviceProvider);

            AutoRegisterRoutes(startUp, serverRoutingTable, serviceProvider);

            startUp.Configure(serverRoutingTable);

            var server = new Server(8000, serverRoutingTable, httpSessionStorage);
            server.Run();

        }

        private static void AutoRegisterRoutes(IMvcApplication startUp
            , IServerRoutingTable serverRoutingTable
            , DependencyContainer.IServiceProvider serviceProvider)
        {
            Assembly applicationAssembly = startUp.GetType().Assembly;

            Type[] controllers = applicationAssembly.GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t)).ToArray();

            foreach (var controller in controllers)
            {
                MethodInfo[] actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance
                    | BindingFlags.DeclaredOnly)
                    .Where(m => m.IsSpecialName == false && m.GetCustomAttribute<NonActionAttribute>() == null).ToArray();
                foreach (var method in actions)
                {
                    BaseHttpAttribute httpAttribute = (BaseHttpAttribute)method
                        .GetCustomAttributes()
                        .Where(a => typeof(BaseHttpAttribute).IsAssignableFrom(a.GetType()))
                        .LastOrDefault();

                    string folderName = controller.Name.Replace("Controller", string.Empty);
                    string actionName = method.Name;

                    string url = $"/{folderName}/{actionName}";

                    HttpRequestMethod httpRequestMethod = HttpRequestMethod.Get;

                    if (httpAttribute != null)
                    {
                        httpRequestMethod = httpAttribute.HttpRequestMethod;

                        if (!string.IsNullOrWhiteSpace(httpAttribute.Url))
                        {
                            url = httpAttribute.Url;
                        }
                        if (!string.IsNullOrWhiteSpace(httpAttribute.ActionName))
                        {
                            actionName = httpAttribute.ActionName;
                            url = $"/{folderName}/{actionName}";
                        }

                    }

                    serverRoutingTable.Add(httpRequestMethod, url, (request)
                          =>
                      {
                          var controllerInstance = serviceProvider.CreateInstance(controller) as Controller;
                          controllerInstance.Request = request;
                          AuthorizeAttribute authorizeAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
                          if (authorizeAttribute != null
                          && !authorizeAttribute.IsAuthorized(controllerInstance.User))
                          {
                              return new RedirectResult("/");
                          }

                          var parametersInfos = method.GetParameters();
                          var parametersInstances = new List<object>();

                          foreach (var parameterInfo in parametersInfos)
                          {
                              var parameterName = parameterInfo.Name;
                              var parameterType = parameterInfo.ParameterType;

                              var parameterValue = GetValue(request, parameterName) as ISet<string>;
                              object parameterValueConverted = null;
                              try
                              {
                                  if (parameterValue == null) // NOT FOUND AND COMPLEX TYPE
                                  {
                                      throw new Exception();
                                  }

                                  if (parameterValue.Count == 1) // SIMPLE TYPE
                                  {
                                      parameterValueConverted = Convert.ChangeType(parameterValue.First(), parameterType);
                                      
                                  }
                                  else // COLLECTION
                                  {
                                      parameterValueConverted = Activator.CreateInstance(parameterType) as IList<string>;
                                      parameterValueConverted = parameterValue.Select(parameter =>
                                      {
                                          Type[] genericArguments = parameterType.GetGenericArguments();
                                          Type conversionType = genericArguments[0];
                                          return Convert.ChangeType(parameter, conversionType);
                                      }).ToList();
                                  }
                              }
                              catch (Exception)
                              {
                                  if (parameterType.GetInterface("IEnumerable") == null)
                                  {
                                      parameterValueConverted = Activator.CreateInstance(parameterType);
                                      foreach (var property in parameterType.GetProperties())
                                      {
                                          var propertyValueFromRequest = GetValue(request, property.Name) as ISet<string>;
                                          object propertyValueFromRequestConverted = null;
                                          if (propertyValueFromRequest.Count == 1)
                                          {
                                              propertyValueFromRequestConverted = Convert.ChangeType(propertyValueFromRequest.First(), property.PropertyType);
                                          }
                                          else
                                          {
                                              propertyValueFromRequestConverted = propertyValueFromRequest.Select(parameter =>
                                              {
                                                  Type[] genericArguments = property.PropertyType.GetGenericArguments();
                                                  Type conversionType = genericArguments[0];
                                                  return Convert.ChangeType(parameter, conversionType);
                                              }).ToList();
                                          }
                                          property.SetValue(parameterValueConverted, propertyValueFromRequestConverted);
                                      } 
                                  }
                              }

                              parametersInstances.Add(parameterValueConverted);
                          }

                          var response = method.Invoke(controllerInstance, parametersInstances.ToArray());
                          return response as IHttpResponse;
                      });



                }
            }
        }

        private static object GetValue(IHttpRequest request, string parameterName)
        {
            object value = null;

            if (request.QueryData.Any(parameter => parameterName.ToLower() == parameter.Key.ToLower()))
            {
                value = request.QueryData.FirstOrDefault(parameter => parameter.Key.ToLower() == parameterName.ToLower()).Value;
            }

            if (request.FormData.Any(parameter => parameterName.ToLower() == parameter.Key.ToLower()))
            {
                value = request.FormData.FirstOrDefault(parameter => parameter.Key.ToLower() == parameterName.ToLower()).Value;
            }

            return value;
        }
    }
}
