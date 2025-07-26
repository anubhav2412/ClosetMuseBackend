using ClosetMuseBackend.DTOs;
using Microsoft.AspNetCore.Http;

namespace ClosetMuseBackend.Services.Interfaces
{
    public interface IOutfitService
    {
        Task<OutfitDto?> GetOutfitByIdAsync(string userId, string outfitId);
        Task<List<OutfitDto>> GetAllOutfitsAsync(string userId);
        Task<OutfitDto> AddOutfitWithImageAsync(string userId, OutfitRequestDto outfitRequestDto, IFormFile image);
        Task UpdateOutfitAsync(string userId, string outfitId, OutfitRequestDto outfitRequestDto);
        Task DeleteOutfitAsync(string userId, string outfitId);
    }
}
