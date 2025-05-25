using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataManager.Core.Json
{
    readonly struct EntityPatchArgs()
    {
        public required string Username { get; init; }
        public bool GeneratePassword { get; init; } = false;
        public string? Password { get; init; }
        public string[]? AclAppend { get; init; }
        public string[]? AclRemove { get; init; }
    }

    readonly struct EntityPatchModifyAccessControlList
    {
        [JsonPropertyName("append")]
        public string[]? Append { get; init; }

        [JsonPropertyName("remove")]
        public string[]? Remove { get; init; }
    }

    readonly struct EntityPatchModify
    {
        [JsonPropertyName("generate_password")]
        public int? GeneratePassword { get; init; }

        [JsonPropertyName("password")]
        public string? Password { get; init; }

        [JsonPropertyName("acl")]
        public EntityPatchModifyAccessControlList? AccessControlList { get; init; }
    }

    readonly struct EntityPatchRequestPayload : IJsonContentSerializable
    {
        [JsonPropertyName("username")]
        public required string Username { get; init; }

        [JsonPropertyName("modify")]
        public required EntityPatchModify Modify { get; init; }

        public static EntityPatchRequestPayload FromArgs(EntityPatchArgs args)
            => new EntityPatchRequestPayload
            {
                Username = args.Username,
                Modify = new EntityPatchModify
                {
                    GeneratePassword = args.GeneratePassword ? 1 : null,
                    Password = args.Password,
                    AccessControlList = new EntityPatchModifyAccessControlList
                    {
                        Append = args.AclAppend,
                        Remove = args.AclRemove
                    }
                }
            };

        public JsonContent SerializeContent() => JsonContent.Create(this);
    }
}
