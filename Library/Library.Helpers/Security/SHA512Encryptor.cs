using System.Security.Cryptography;
using System.Text;

namespace Library.Helpers.Security
{
    public abstract class SHA512Encryptor
    {
        public static string Encrypt(string value)
        {
            value = "secret" + value + "secret";
            SHA512 sha = SHA512.Create();
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] sha512Bytes = sha.ComputeHash(valueBytes);
            return HashToByte(sha512Bytes);
        }

        private static string HashToByte(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte item in hash)
                result.Append(item.ToString("x2"));

            return result.ToString();
        }
    }
}
