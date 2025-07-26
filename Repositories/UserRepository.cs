using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreDb _db;
        public UserRepository(FirestoreDb db) => _db = db;

        public async Task AddUserAsync(User user)
        {
            user.CreatedAt = Timestamp.GetCurrentTimestamp();
            await _db.Collection("users").Document(user.Id).SetAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var doc = await _db.Collection("users").Document(userId).GetSnapshotAsync();
            return doc.Exists ? doc.ConvertTo<User>() : null;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _db.Collection("users").Document(user.Id).SetAsync(user, SetOptions.MergeAll);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _db.Collection("users").Document(userId).DeleteAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var snapshot = await _db.Collection("users").GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<User>()).ToList();
        }
    }
}
