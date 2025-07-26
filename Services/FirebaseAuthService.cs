using FirebaseAdmin.Auth;

namespace ClosetMuseBackend.Services
{
    public class FirebaseAuthService
    {
        public async Task<string> CreateFirebaseUserAsync(string email, string password, string displayName)
        {
            try
            {
                var firebaseUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
                {
                    Email = email,
                    Password = password,
                    DisplayName = displayName
                });

                return firebaseUser.Uid; // Return the Firebase UID
            }
            catch (FirebaseAuthException ex)
            {
                // Log the error and rethrow it
                Console.WriteLine($"Error creating Firebase user: {ex.Message}");
                throw;
            }
        }
    }
}
