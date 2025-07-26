using ClosetMuseBackend.Models;
using ClosetMuseBackend.Repositories.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace ClosetMuseBackend.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly FirebaseAuthService _authService;
        private readonly StorageClient _storage;
        private readonly string _bucketName;

        public UserService(IUserRepository repo, FirebaseAuthService authService, StorageClient storage, string bucketName)
        {
            _repo = repo;
            _authService = authService;
            _storage = storage;
            _bucketName = bucketName;
        }

        public async Task AddUserAsync(User user, string password)
        {
            // Create user in Firebase Authentication
            user.Id = await _authService.CreateFirebaseUserAsync(user.Email, password, user.Name);

            // Continue creating the user in Firestore
            await _repo.AddUserAsync(user);
        }

        public Task<User?> GetUserByIdAsync(string userId) => _repo.GetUserByIdAsync(userId);
        public Task UpdateUserAsync(User user) => _repo.UpdateUserAsync(user);
        public Task DeleteUserAsync(string userId) => _repo.DeleteUserAsync(userId);
        public Task<List<User>> GetAllUsersAsync() => _repo.GetAllUsersAsync();

        public async Task UpdateProfileImageAsync(string userId, IFormFile image)
        {
            var user = await _repo.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Delete old image if it exists and is not a literal placeholder
            if (!string.IsNullOrEmpty(user.ProfileImageUrl) && !user.ProfileImageUrl.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var urlParts = user.ProfileImageUrl.Split("/o/");
                    if (urlParts.Length > 1)
                    {
                        var oldObjectName = urlParts[1].Split("?")[0];
                        var oldObjectNameDecoded = Uri.UnescapeDataString(oldObjectName);

                        // Check if the object exists in the bucket
                        try
                        {
                            await _storage.GetObjectAsync(_bucketName, oldObjectNameDecoded);
                            // If the object exists, delete it
                            await _storage.DeleteObjectAsync(_bucketName, oldObjectNameDecoded);
                        }
                        catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
                        {
                            Console.WriteLine($"Old profile image not found: {oldObjectNameDecoded}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete old profile image: {ex.Message}");
                    // Optionally, log the error or handle it as needed
                }
            }

            // Upload new image
            var objectName = $"users/{userId}/profile.jpg";
            using var stream = image.OpenReadStream();
            await _storage.UploadObjectAsync(_bucketName, objectName, image.ContentType, stream);

            // Generate public URL
            user.ProfileImageUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";

            // Update user in Firestore
            await _repo.UpdateUserAsync(user);
        }
    }
}
