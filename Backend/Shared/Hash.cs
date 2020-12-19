using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Shared
{
    public class Hash
    {
        public static string Create(string value, byte[] salt)
        {
            var hashed = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hashed);
        }

        public static bool Validate(string value, byte[] salt, string hash)
            => Create(value, salt) == hash;
    }
}
