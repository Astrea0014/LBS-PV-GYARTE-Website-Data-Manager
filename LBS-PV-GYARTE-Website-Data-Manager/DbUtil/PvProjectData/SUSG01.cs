using System.Text.Json.Serialization;

namespace DataManager.DbUtil.PvProjectData
{
    internal struct SUSG01 : IProjectData
    {
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }

        [JsonPropertyName("itch_href")]
        public string ItchHref { get; set; }

        [JsonPropertyName("video_ref")]
        public string VideoRef { get; set; }

        [JsonPropertyName("moodboard_ref")]
        public string MoodboardRef { get; set; }
    }
}
