using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions.Json
{
    readonly struct FailureResponsePayload : IExpectedServerException
    {
        [JsonPropertyName("error")]
        public required string ServerExceptionMessage { get; init; }
    }
}
