using SIS.HTTP.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.SecurityAttributes
{
    public class AuthorizeAttribute : Attribute
    {
        private string authority;

        public AuthorizeAttribute(string authority = "authorized")
        {
            this.authority = authority;
        }

        public bool IsLogedIn(Principal principal)
        {
            return principal != null;
        }

        public bool IsAuthorized(Principal principal)
        {
            if (IsLogedIn(principal) == false)
            {
                return this.authority == "anonymous";
            }

            return this.authority == "authorized"
                || principal.Roles.Contains(this.authority.ToLower());
        }
    }
}
