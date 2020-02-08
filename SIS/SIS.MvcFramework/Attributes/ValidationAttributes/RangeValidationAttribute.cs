using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.MvcFramework.Attributes.ValidationAttributes
{
    public class RangeValidationAttribute : BaseValidationAttribute
    {
        private readonly object minValue;
        private readonly object maxValue;

        public RangeValidationAttribute(object minValue, object maxValue, string errorMessage) : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            object newMinValueCasted = Convert.ChangeType(minValue,value.GetType());
            object newMaxValueCasted = Convert.ChangeType(maxValue, value.GetType());

            var compareToMethod = value.GetType().GetMethods()
                .Where(m => m.Name == "CompareTo"
                && m.GetParameters().First().ParameterType == typeof(object)).First();

            return (int)compareToMethod.Invoke(newMinValueCasted,new object[] { value}) < 0
                && (int)compareToMethod.Invoke(newMaxValueCasted, new object[] { value }) > 0;
        }
    }
}
