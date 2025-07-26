using Google.Cloud.Firestore;

namespace ClosetMuseBackend.DTOs
{
    public class OutfitDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Cost { get; set; }
        public string Type { get; set; }
        public List<string> Tags { get; set; }
        public string CreatedAt { get; set; } // Serialize as ISO 8601 string
    }
}
