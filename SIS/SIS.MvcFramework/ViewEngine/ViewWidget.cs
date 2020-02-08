using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SIS.MvcFramework.ViewEngine
{
    public class ViewWidget : IViewWidget
    {
        private const string FolderPath = "Views/Shared/Widgets/";
        private const string WidgetFileExtension = ".vwhtml";
        public string GetContent()
        {
            return File.ReadAllText(
                $"{FolderPath}{this.GetType().Name}{WidgetFileExtension}");
        }
    }
}
