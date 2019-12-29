using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;

namespace SIS.MvcFramework.Attributes.HttpAttributes
{
    public class HttpPutAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod HttpRequestMethod => HttpRequestMethod.Put;
    }
}
