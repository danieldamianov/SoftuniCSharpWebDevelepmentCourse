﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.DependencyContainer
{
    public interface IServiceProvider
    {
        void Add<TSource, TDestination>()
            where TDestination : TSource;

        object CreateInstance(Type type);
    }
}
