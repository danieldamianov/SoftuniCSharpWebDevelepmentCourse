using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException() : base("The Server has encountered an error.")
        {

        }
    }
}
