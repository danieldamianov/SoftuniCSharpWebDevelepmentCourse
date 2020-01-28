using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.MvcFramework.DependencyContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, Type> container;

        public ServiceProvider()
        {
            this.container = new ConcurrentDictionary<Type, Type>();
        }
        public void Add<TSource, TDestination>() where TDestination : TSource
        {
            this.container[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            if (this.container.ContainsKey(type))
            {
                type = this.container[type];
            }

            var constructor = type.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .OrderBy(contructor => contructor.GetParameters().Length)
                .First();

            List<object> parameters = new List<object>();

            foreach (var param in constructor.GetParameters())
            {
                parameters.Add(CreateInstance(param.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }
    }
}
