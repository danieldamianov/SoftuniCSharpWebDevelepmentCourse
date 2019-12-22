using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string text)
        {
            return text[0].ToString().ToUpper() + text.Substring(1).ToLower();
        }
    }
}

