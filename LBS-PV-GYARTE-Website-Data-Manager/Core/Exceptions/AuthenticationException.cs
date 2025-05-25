using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class AuthenticationException : Exception, IExpectedServerException
    {
        public AuthenticationException() : base("The client has an unspecified issue with their authorization towards the server.") { }
        public AuthenticationException(string? message) : base(message) { }
        public AuthenticationException(string? message, Exception? innerException) : base(message, innerException) { }

        public virtual string ServerExceptionMessage { get; init; }
    }
}
