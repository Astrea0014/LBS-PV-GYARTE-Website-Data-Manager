using System.Text.Json.Serialization;

namespace DataManager.DbUtil.GyProjectData
{
    internal struct ES2 : IComponentData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("video_ref")]
        public string VideoReference { get; set; }

        [JsonPropertyName("images")]
        public Image[] Images { get; set; }
    }
}
