namespace ClosetMuseBackend.DTOs
{
    public class PlanDto
    {
        public string Date { get; set; }
        public List<string> OutfitIds { get; set; }
        public double TotalCost { get; set; }
        public string Notes { get; set; }
    }
}
