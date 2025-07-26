using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Models
{
    [FirestoreData]
    public class Plan
    {
        [FirestoreProperty] public string Id { get; set; }
        [FirestoreProperty] public string Date { get; set; }
        [FirestoreProperty] public List<string> OutfitIds { get; set; }
        [FirestoreProperty] public double TotalCost { get; set; }
        [FirestoreProperty] public string Notes { get; set; }
        [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    }
}
