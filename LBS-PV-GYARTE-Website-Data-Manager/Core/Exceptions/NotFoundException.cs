using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class NotFoundException : Exception, IExpectedServerException
    {
        public NotFoundException() : base("The requested resource could not be found.") { }
        public NotFoundException(string? message) : base(message) { }
        public NotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

        public required string ServerExceptionMessage { get; init; }
    }
}
