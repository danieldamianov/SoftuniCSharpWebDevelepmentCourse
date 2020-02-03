using SIS.HTTP.Identity;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework.ViewEngine
{
    public interface IViewEngine
    {
        string TransformView<T>(string viewContent, T model, ModelStateDictionary modelStateDictionary,Principal user);
    }
}
