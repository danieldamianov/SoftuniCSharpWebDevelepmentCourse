using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Sessions
{
    class HttpSession : IHttpSession
    {
        private Dictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            if (this.ContainsParameter(name) == false)
            {
                throw new ArgumentException("Such parameter already exists!!!");
            }

            this.parameters[name] = parameter;
        }

        public void ClearParameters()
        {
            this.parameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            if (this.ContainsParameter(name) == false)
            {
                throw new ArgumentException("No such parameter exists!");
            }

            return this.parameters[name];
        }
    }
}
