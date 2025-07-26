using ClosetMuseBackend.Models; // Ensure this is included
using ClosetMuseBackend.Services.Interfaces;
using ClosetMuseBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClosetMuseBackend.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/outfits")]
    public class OutfitsController : ControllerBase
    {
        private readonly IOutfitService _service;

        public OutfitsController(IOutfitService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOutfit(string userId, string id)
        {
            var outfit = await _service.GetOutfitByIdAsync(userId, id);
            return outfit is not null ? Ok(outfit) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOutfits(string userId)
        {
            var outfits = await _service.GetAllOutfitsAsync(userId);
            return Ok(outfits);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> AddOutfitWithImage(string userId, [FromForm] OutfitRequestDto outfitRequestDto, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image file is required.");

            var outfit = await _service.AddOutfitWithImageAsync(userId, outfitRequestDto, image);
            return CreatedAtAction(nameof(GetOutfit), new { userId, id = outfit.Id }, outfit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOutfit(string userId, string id, [FromBody] OutfitRequestDto outfitRequestDto)
        {
            await _service.UpdateOutfitAsync(userId, id, outfitRequestDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutfit(string userId, string id)
        {
            await _service.DeleteOutfitAsync(userId, id);
            return NoContent();
        }
    }
}
