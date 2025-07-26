using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Models
{
    [FirestoreData]
    public class Outfit
    {
        [FirestoreProperty] public string Id { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Description { get; set; }
        [FirestoreProperty] public string ImageUrl { get; set; }
        [FirestoreProperty] public double Cost { get; set; }
        [FirestoreProperty] public string Type { get; set; }
        [FirestoreProperty] public List<string> Tags { get; set; }
        [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    }
}
