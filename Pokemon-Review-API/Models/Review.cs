using System.Text.Json.Serialization;

namespace Pokemon_Review_API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; internal set; }
        [JsonIgnore]
        public Reviewer Reviewer { get; set; }
        [JsonIgnore]
        public Pokemon Pokemon { get; set; }
    }
}
