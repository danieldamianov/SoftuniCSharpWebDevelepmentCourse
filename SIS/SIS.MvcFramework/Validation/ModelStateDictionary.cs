using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SIS.MvcFramework.Validation
{
    public class ModelStateDictionary
    {
        private readonly IDictionary<string, List<string>> errorMessages;

        public bool IsValid => this.errorMessages.Count == 0;

        public ModelStateDictionary()
        {
            this.errorMessages = new Dictionary<string, List<string>>();
        }

        public IReadOnlyDictionary<string, List<string>> ErrorMessages
            => errorMessages.ToImmutableDictionary();

        public void Add(string propName, string message)
        {
            if (this.errorMessages.ContainsKey(propName) == false)
            {
                this.errorMessages.Add(propName, new List<string>());
            }

            this.errorMessages[propName].Add(message);
        }
    }
}
