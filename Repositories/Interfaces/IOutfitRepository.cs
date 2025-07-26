using ClosetMuseBackend.Models;

namespace ClosetMuseBackend.Repositories.Interfaces
{
    public interface IOutfitRepository
    {
        Task AddOutfitAsync(string userId, Outfit outfit);
        Task<Outfit?> GetOutfitByIdAsync(string userId, string outfitId);
        Task UpdateOutfitAsync(string userId, Outfit outfit);
        Task DeleteOutfitAsync(string userId, string outfitId);
        Task<List<Outfit>> GetAllOutfitsAsync(string userId);
        Task<List<Outfit>> GetOutfitsByTagsAsync(string userId, List<string> tags);
        Task<List<Outfit>> GetOutfitsByTypeAsync(string userId, string type);
    }
}
