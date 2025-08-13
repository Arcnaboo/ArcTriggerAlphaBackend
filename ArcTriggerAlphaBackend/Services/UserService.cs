using ArcTriggerAlphaBackend.Database.Interfaces;
using ArcTriggerAlphaBackend.Entities;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArcTriggerAlphaBackend.Services
{
    public class UserService
    {
        private readonly IMultilRepository _repo;

        public UserService(IMultilRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<UserCreationResult> CreateUserAsync(string email, string plainPassword)
        {
            if (await _repo.UserExistsAsync(email))
                return UserCreationResult.EmailExists;

            var user = new User(
                email: email,
                passwordHash: HashPassword(plainPassword)
            );

            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            return UserCreationResult.Success;
        }

        public async Task<AuthResult> AuthenticateAsync(string email, string plainPassword)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) return AuthResult.InvalidCredentials;

            if (!VerifyPassword(plainPassword, user.Password))
                return AuthResult.InvalidCredentials;

            return AuthResult.Success(Guid.NewGuid().ToString());
        }

        public enum UserCreationResult { Success, EmailExists }

        public class AuthResult
        {
            public bool IsSuccess { get; }
            public string SessionKey { get; }
            public static AuthResult Success(string key) => new AuthResult(true, key);
            public static AuthResult InvalidCredentials = new AuthResult(false, null);

            private AuthResult(bool success, string key)
                => (IsSuccess, SessionKey) = (success, key);
        }

        // Password hashing (no salt)
        private string HashPassword(string plainPassword)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
            => HashPassword(inputPassword) == storedHash;
    }
}