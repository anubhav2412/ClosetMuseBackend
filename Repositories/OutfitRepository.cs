using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Repositories
{
    public class OutfitRepository : IOutfitRepository
    {
        private readonly FirestoreDb _db;
        public OutfitRepository(FirestoreDb db) => _db = db;

        public async Task AddOutfitAsync(string userId, Outfit outfit)
        {
            outfit.CreatedAt = Timestamp.GetCurrentTimestamp();
            await _db.Collection($"users/{userId}/outfits").Document(outfit.Id).SetAsync(outfit);
        }

        public async Task<Outfit?> GetOutfitByIdAsync(string userId, string outfitId)
        {
            var doc = await _db.Collection($"users/{userId}/outfits").Document(outfitId).GetSnapshotAsync();
            if (doc.Exists)
            {
                var outfit = doc.ConvertTo<Outfit>();
                outfit.CreatedAt = doc.ContainsField("CreatedAt") 
                    ? doc.GetValue<Timestamp>("CreatedAt") 
                    : Timestamp.FromDateTime(DateTime.UtcNow); // Default to current timestamp if missing
                return outfit;
            }
            return null;
        }

        public async Task UpdateOutfitAsync(string userId, Outfit outfit)
        {
            await _db.Collection($"users/{userId}/outfits").Document(outfit.Id).SetAsync(outfit, SetOptions.MergeAll);
        }

        public async Task DeleteOutfitAsync(string userId, string outfitId)
        {
            await _db.Collection($"users/{userId}/outfits").Document(outfitId).DeleteAsync();
        }

        public async Task<List<Outfit>> GetAllOutfitsAsync(string userId)
        {
            var snapshot = await _db.Collection($"users/{userId}/outfits").GetSnapshotAsync();
            return snapshot.Documents.Select(doc =>
            {
                var outfit = doc.ConvertTo<Outfit>();
                outfit.CreatedAt = doc.ContainsField("CreatedAt") 
                    ? doc.GetValue<Timestamp>("CreatedAt") 
                    : Timestamp.FromDateTime(DateTime.UtcNow); // Default to current timestamp if missing
                return outfit;
            }).ToList();
        }

        public async Task<List<Outfit>> GetOutfitsByTagsAsync(string userId, List<string> tags)
        {
            var query = _db.Collection($"users/{userId}/outfits").WhereArrayContainsAny("Tags", tags);
            var snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Select(doc =>
            {
                var outfit = doc.ConvertTo<Outfit>();
                outfit.CreatedAt = doc.ContainsField("CreatedAt") 
                    ? doc.GetValue<Timestamp>("CreatedAt") 
                    : Timestamp.FromDateTime(DateTime.UtcNow); // Default to current timestamp if missing
                return outfit;
            }).ToList();
        }

        public async Task<List<Outfit>> GetOutfitsByTypeAsync(string userId, string type)
        {
            var query = _db.Collection($"users/{userId}/outfits").WhereEqualTo("Type", type);
            var snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Select(doc =>
            {
                var outfit = doc.ConvertTo<Outfit>();
                outfit.CreatedAt = doc.ContainsField("CreatedAt") 
                    ? doc.GetValue<Timestamp>("CreatedAt") 
                    : Timestamp.FromDateTime(DateTime.UtcNow); // Default to current timestamp if missing
                return outfit;
            }).ToList();
        }
    }
}
