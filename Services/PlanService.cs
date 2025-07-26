using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using ClosetMuseBackend.Services.Interfaces;

namespace ClosetMuseBackend.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _repo;

        public PlanService(IPlanRepository repo)
        {
            _repo = repo;
        }

        public Task AddPlanAsync(string userId, Plan plan) => _repo.AddPlanAsync(userId, plan);

        public Task UpdatePlanAsync(string userId, Plan plan) => _repo.UpdatePlanAsync(userId, plan);

        public Task DeletePlanAsync(string userId, string planId) => _repo.DeletePlanAsync(userId, planId);

        public Task<List<Plan>> GetPlansByDateAsync(string userId, string date) => _repo.GetPlansByDateAsync(userId, date);

        public Task<List<Plan>> GetPlansByMonthAsync(string userId, string yearMonth) => _repo.GetPlansByMonthAsync(userId, yearMonth);

        public async Task<List<Plan>> GetAllPlansAsync(string userId)
        {
            // Delegate the call to the repository to fetch all plans for the user
            return await _repo.GetAllPlansAsync(userId);
        }

        public Task<Plan?> GetPlanByIdAsync(string userId, string planId) => _repo.GetPlanByIdAsync(userId, planId);
    }
}
