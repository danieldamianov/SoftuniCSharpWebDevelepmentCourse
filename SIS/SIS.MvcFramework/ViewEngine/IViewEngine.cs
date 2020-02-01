using SIS.HTTP.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.ViewEngine
{
    public interface IViewEngine
    {
        string TransformView<T>(string viewContent, T model,Principal user);
    }
}
