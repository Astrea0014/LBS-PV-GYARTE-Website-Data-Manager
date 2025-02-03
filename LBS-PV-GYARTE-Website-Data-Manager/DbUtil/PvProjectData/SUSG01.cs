using System.Text.Json.Serialization;

namespace DataManager.DbUtil.PvProjectData
{
    internal struct SUSG01 : IProjectData
    {
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }

        [JsonPropertyName("itch_href")]
        public string ItchHyperReference { get; set; }

        [JsonPropertyName("video_ref")]
        public string VideoReference { get; set; }

        [JsonPropertyName("moodboard_ref")]
        public string MoodboardReference { get; set; }
    }
}
