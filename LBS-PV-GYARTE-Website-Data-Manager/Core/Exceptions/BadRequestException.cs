using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    class BadRequestException : HttpRequestException, IExpectedServerException
    {
        public BadRequestException() : base("The request was ill-formatted or was missing crucial information. Ensure the request headers and body comply with the endpoint specifications, and try again.", null, HttpStatusCode.BadRequest) { }
        public BadRequestException(string? message) : base(message, null, HttpStatusCode.BadRequest) { }
        public BadRequestException(string? message, Exception? innerException) : base(message, innerException, HttpStatusCode.BadRequest) { }

        public required string ServerExceptionMessage { get; init; }
    }
}
