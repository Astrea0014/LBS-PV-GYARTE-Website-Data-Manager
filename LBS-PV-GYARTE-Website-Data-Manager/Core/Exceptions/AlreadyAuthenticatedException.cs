using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class AlreadyAuthenticatedException : AuthenticationException
    {
        public AlreadyAuthenticatedException() : base("Cannot authorize connection; this instance is already authorized.") { }
        public AlreadyAuthenticatedException(string? message) : base(message) { }
        public AlreadyAuthenticatedException(string? message, Exception? innerException) : base(message, innerException) { }

        public override string ServerExceptionMessage => "Unset";
    }
}
