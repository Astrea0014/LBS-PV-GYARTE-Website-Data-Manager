using System.Text.Json.Serialization;

namespace DataManager.DbUtil.GyProjectData
{
    internal struct ES1 : IComponentData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("images")]
        public Image[] Images { get; set; }
    }
}
