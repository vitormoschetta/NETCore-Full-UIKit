using System.Security.Cryptography;

namespace Shared
{
    public class Salt
    {
        public static byte[] Create()
        {
            byte[] salt = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
                return salt;
            }
        }
    }
}