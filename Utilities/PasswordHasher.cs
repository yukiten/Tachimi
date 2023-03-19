using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Tachimi.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            string passwordToVerify = HashPassword(password, salt);
            return passwordToVerify == hashedPassword;
        }
    }
}
