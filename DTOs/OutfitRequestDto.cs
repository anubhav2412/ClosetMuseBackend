namespace ClosetMuseBackend.DTOs
{
    public class OutfitRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string Type { get; set; }
        public List<string> Tags { get; set; }
    }
}
