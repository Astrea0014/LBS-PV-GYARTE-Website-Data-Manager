using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class ServerException : Exception, IExpectedServerException
    {
        public ServerException() : base("The server had an internal error.") { }
        public ServerException(string? message) : base(message) { }
        public ServerException(string? message, Exception? innerException) : base(message, innerException) { }

        public required string ServerExceptionMessage { get; init; }
    }
}
