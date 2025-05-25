using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataManager.Core.Json
{
    readonly struct Access
    {
        [JsonPropertyName("access_id")]
        public required string AccessId { get; init; }

        [JsonPropertyName("description")]
        public required string Description { get; init; }
    }

    readonly struct EntityResponsePayload
    {
        [JsonPropertyName("username")]
        public required string Username { get; init; }

        [JsonPropertyName("acl")]
        public required Access[] AccessControlList { get; init; }
    }
}
