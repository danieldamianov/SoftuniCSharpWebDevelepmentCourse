using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseValidationAttribute : Attribute
    {
        public BaseValidationAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public abstract bool IsValid(object value);
    }
}
