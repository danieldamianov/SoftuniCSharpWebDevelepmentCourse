using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
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
            AutoRegisterRoutes(startUp, serverRoutingTable);
            startUp.Configure(serverRoutingTable);
            startUp.ConfigureServices();

            var server = new Server(8000, serverRoutingTable);
            server.Run();

        }

        private static void AutoRegisterRoutes(IMvcApplication startUp, IServerRoutingTable serverRoutingTable)
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
                          var controllerInstance = Activator.CreateInstance(controller);
                          ((Controller)controllerInstance).Request = request;
                          AuthorizeAttribute authorizeAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
                          if (authorizeAttribute != null
                          && !authorizeAttribute.IsAuthorized(((Controller)controllerInstance).User))
                          {
                              return new RedirectResult("/");
                          }
                          var response = method.Invoke(controllerInstance, new object[0] { });
                          return response as IHttpResponse;
                      });



                }
            }
        }
    }
}
