using ClosetMuseBackend.Models;

namespace ClosetMuseBackend.Services.Interfaces
{
    public interface IPlanService
    {
        Task AddPlanAsync(string userId, Plan plan);
        Task UpdatePlanAsync(string userId, Plan plan);
        Task DeletePlanAsync(string userId, string planId);
        Task<List<Plan>> GetPlansByDateAsync(string userId, string date);
        Task<List<Plan>> GetPlansByMonthAsync(string userId, string yearMonth);
        Task<List<Plan>> GetAllPlansAsync(string userId);
        Task<Plan?> GetPlanByIdAsync(string userId, string planId);
    }
}
