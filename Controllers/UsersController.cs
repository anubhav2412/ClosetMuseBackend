using ClosetMuseBackend.Models;
using ClosetMuseBackend.Services;
using ClosetMuseBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ClosetMuseBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        public UsersController(UserService service) => _service = service;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _service.GetUserByIdAsync(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Password is required.");

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                ProfileImageUrl = userDto.ProfileImageUrl
            };

            await _service.AddUserAsync(user, password);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User user)
        {
            user.Id = id;
            await _service.UpdateUserAsync(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _service.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/profile-image")]
        public async Task<IActionResult> UpdateProfileImage(string id, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image file is required.");

            await _service.UpdateProfileImageAsync(id, image);
            return Ok("Profile image updated successfully.");
        }
    }
}
