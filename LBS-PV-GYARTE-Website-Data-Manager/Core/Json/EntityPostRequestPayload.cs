using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataManager.Core.Json
{
    readonly struct EntityPostRequestPayload : IJsonContentSerializable
    {
        [JsonPropertyName("username")]
        public required string Username { get; init; }

        [JsonPropertyName("password")]
        public string? Password { get; init; }

        [JsonPropertyName("access")]
        public required string[] AccessNames { get; init; }

        public JsonContent SerializeContent()
            => JsonContent.Create(this,
                options: new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
    }
}
