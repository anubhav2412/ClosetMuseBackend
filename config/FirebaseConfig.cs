using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;

namespace ClosetMuseBackend.Config
{
    public static class FirebaseConfig
    {
        public static FirestoreDb InitializeFirestore(string serviceAccountPath, string projectId)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(serviceAccountPath)
                });
            }
            return FirestoreDb.Create(projectId);
        }

        public static StorageClient InitializeStorage()
        {
            return StorageClient.Create();
        }
    }
}
