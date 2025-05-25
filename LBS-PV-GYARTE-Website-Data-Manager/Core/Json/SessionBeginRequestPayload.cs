using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataManager.Core.Json
{
    readonly struct SessionBeginRequestPayload : IJsonContentSerializable
    {
        [JsonPropertyName("master")]
        public required string Master { get; init; }

        public JsonContent SerializeContent() => JsonContent.Create(this);
    }
}
