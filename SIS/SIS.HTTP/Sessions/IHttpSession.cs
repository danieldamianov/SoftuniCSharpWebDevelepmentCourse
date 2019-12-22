using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Sessions
{
    public interface IHttpSession
    {
        string Id { get; }

        void AddParameter(string name,object parameter);

        bool ContainsParameter(string name);

        object GetParameter(string name);

        void ClearParameters();
    }
}
