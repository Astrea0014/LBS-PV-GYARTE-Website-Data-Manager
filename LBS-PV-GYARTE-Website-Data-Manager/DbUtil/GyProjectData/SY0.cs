using System.Text.Json.Serialization;

namespace DataManager.DbUtil.GyProjectData
{
    internal struct SY0 : IComponentData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("href")]
        public string HyperReference { get; set; }
    }
}
