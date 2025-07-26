using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty] public string Id { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Email { get; set; }
        [FirestoreProperty] public string? ProfileImageUrl { get; set; }
        [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    }
}
