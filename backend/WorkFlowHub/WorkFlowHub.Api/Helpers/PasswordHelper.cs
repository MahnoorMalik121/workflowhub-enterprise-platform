using System.Security.Cryptography;
using System.Text;

namespace WorkFlowHub.Api.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(passwordBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            var hashedPassword = HashPassword(password);

            return hashedPassword == passwordHash;
        }
    }
}