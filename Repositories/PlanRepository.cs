using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using Google.Cloud.Firestore;

namespace ClosetMuseBackend.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly FirestoreDb _db;
        public PlanRepository(FirestoreDb db) => _db = db;

        public async Task AddPlanAsync(string userId, Plan plan)
        {
            plan.CreatedAt = Timestamp.GetCurrentTimestamp();
            await _db.Collection($"users/{userId}/plans").Document(plan.Id).SetAsync(plan);
        }

        public async Task<Plan?> GetPlanByIdAsync(string userId, string planId)
        {
            var doc = await _db.Collection($"users/{userId}/plans").Document(planId).GetSnapshotAsync();
            return doc.Exists ? doc.ConvertTo<Plan>() : null;
        }

        public async Task UpdatePlanAsync(string userId, Plan plan)
        {
            await _db.Collection($"users/{userId}/plans").Document(plan.Id).SetAsync(plan, SetOptions.MergeAll);
        }

        public async Task DeletePlanAsync(string userId, string planId)
        {
            await _db.Collection($"users/{userId}/plans").Document(planId).DeleteAsync();
        }

        public async Task<List<Plan>> GetPlansByDateAsync(string userId, string date)
        {
            var query = _db.Collection($"users/{userId}/plans").WhereEqualTo("Date", date);
            var snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<Plan>()).ToList();
        }

        public async Task<List<Plan>> GetPlansByMonthAsync(string userId, string yearMonth)
        {
            var query = _db.Collection($"users/{userId}/plans").WhereGreaterThanOrEqualTo("Date", $"{yearMonth}-01")
                .WhereLessThanOrEqualTo("Date", $"{yearMonth}-31");
            var snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<Plan>()).ToList();
        }

        public async Task<List<Plan>> GetAllPlansAsync(string userId)
        {
            var snapshot = await _db.Collection($"users/{userId}/plans").GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<Plan>()).ToList();
        }
    }
}
