using ClosetMuseBackend.Models;

namespace ClosetMuseBackend.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task AddPlanAsync(string userId, Plan plan);
        Task UpdatePlanAsync(string userId, Plan plan);
        Task DeletePlanAsync(string userId, string planId);
        Task<List<Plan>> GetPlansByDateAsync(string userId, string date);       // e.g., "2025-07-30"
        Task<List<Plan>> GetPlansByMonthAsync(string userId, string yearMonth); // e.g., "2025-07"
        Task<List<Plan>> GetAllPlansAsync(string userId);
        Task<Plan?> GetPlanByIdAsync(string userId, string planId);
    }
}
