using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.ValidationAttributes
{
    public class StringLengthValidationAttribute : BaseValidationAttribute
    {
        private readonly int minValue;
        private readonly int maxValue;

        public StringLengthValidationAttribute(string errorMessage, int minValue, int maxValue) : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            string valueAsString = (string)value;

            int stringLength = valueAsString.Length;

            return stringLength >= minValue && stringLength <= maxValue;
        }
    }
}
