using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class AuthorizationException : Exception, IExpectedServerException
    {
        public AuthorizationException() : base("The current session does not have the authorization needed to access this resource.") { }
        public AuthorizationException(string? message) : base(message) { }
        public AuthorizationException(string? message, Exception? innerException) : base(message, innerException) { }

        public required string ServerExceptionMessage { get; init; }
    }
}
