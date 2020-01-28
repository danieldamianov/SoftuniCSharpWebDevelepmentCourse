using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Sessions
{
    public interface IHttpSessionStorage
    {
        IHttpSession GetSession(string id);
    }
}
