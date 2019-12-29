using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;

namespace SIS.MvcFramework.Results
{
    public class NotFoundResult : ActionResult
    {
        public NotFoundResult(string message, HttpResponseStatusCode httpResponseStatusCode) : base(HttpResponseStatusCode.NotFound)
        {
            this.Content = Encoding.UTF8.GetBytes(message);
        }
    }
}
