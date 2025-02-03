using System.Text.Json.Serialization;

namespace DataManager.DbUtil.GyProjectData
{
    internal struct Image
    {
        [JsonPropertyName("image_header")]
        public string Header { get; set; }

        [JsonPropertyName("image_ref")]
        public string Reference { get; set; }

        [JsonPropertyName("image_format")]
        public string Format { get; set; }
    }
}
