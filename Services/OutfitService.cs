using Google.Cloud.Storage.V1;
using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using ClosetMuseBackend.Services.Interfaces;
using ClosetMuseBackend.DTOs;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ClosetMuseBackend.Services
{
    public class OutfitService : IOutfitService
    {
        private readonly IOutfitRepository _repo;
        private readonly StorageClient _storage;
        private readonly string _bucketName;

        public OutfitService(IOutfitRepository repo, StorageClient storage, string bucketName)
        {
            _repo = repo;
            _storage = storage;
            _bucketName = bucketName;
        }

        public async Task<OutfitDto?> GetOutfitByIdAsync(string userId, string outfitId)
        {
            var outfit = await _repo.GetOutfitByIdAsync(userId, outfitId);
            return outfit is not null ? new OutfitDto
            {
                Id = outfit.Id,
                Name = outfit.Name,
                Description = outfit.Description,
                ImageUrl = outfit.ImageUrl,
                Cost = outfit.Cost,
                Type = outfit.Type,
                Tags = outfit.Tags,
                CreatedAt = outfit.CreatedAt.ToDateTime().ToString("o")
            } : null;
        }

        public async Task<List<OutfitDto>> GetAllOutfitsAsync(string userId)
        {
            var outfits = await _repo.GetAllOutfitsAsync(userId);
            return outfits.Select(o => new OutfitDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                ImageUrl = o.ImageUrl,
                Cost = o.Cost,
                Type = o.Type,
                Tags = o.Tags,
                CreatedAt = o.CreatedAt.ToDateTime().ToString("o")
            }).ToList();
        }

        public async Task<OutfitDto> AddOutfitWithImageAsync(string userId, OutfitRequestDto outfitRequestDto, IFormFile image)
        {
            var outfit = new Outfit
            {
                Id = Guid.NewGuid().ToString(),
                Name = outfitRequestDto.Name,
                Description = outfitRequestDto.Description,
                Cost = outfitRequestDto.Cost,
                Type = outfitRequestDto.Type,
                Tags = outfitRequestDto.Tags
            };

            // Upload image to Firebase Storage
            string objectName = $"users/{userId}/outfits/{outfit.Id}/main.jpg";
            using var stream = image.OpenReadStream();
            await _storage.UploadObjectAsync(_bucketName, objectName, image.ContentType, stream);

            // Generate public URL
            outfit.ImageUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
            outfit.CreatedAt = Google.Cloud.Firestore.Timestamp.GetCurrentTimestamp();

            // Save outfit metadata to Firestore
            await _repo.AddOutfitAsync(userId, outfit);

            return new OutfitDto
            {
                Id = outfit.Id,
                Name = outfit.Name,
                Description = outfit.Description,
                ImageUrl = outfit.ImageUrl,
                Cost = outfit.Cost,
                Type = outfit.Type,
                Tags = outfit.Tags,
                CreatedAt = outfit.CreatedAt.ToDateTime().ToString("o")
            };
        }

        public async Task UpdateOutfitAsync(string userId, string outfitId, OutfitRequestDto outfitRequestDto)
        {
            var outfit = new Outfit
            {
                Id = outfitId,
                Name = outfitRequestDto.Name,
                Description = outfitRequestDto.Description,
                Cost = outfitRequestDto.Cost,
                Type = outfitRequestDto.Type,
                Tags = outfitRequestDto.Tags
            };

            await _repo.UpdateOutfitAsync(userId, outfit);
        }

        public Task DeleteOutfitAsync(string userId, string outfitId) => _repo.DeleteOutfitAsync(userId, outfitId);
    }
}
