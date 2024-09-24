using System.Security.Cryptography;
using System.Text;

namespace BusParkDispatcher.Infrastructure
{
    static class SecurityCryptography
    {
        #region Properties
        public static SHA256 SHA256 { set; get; } = SHA256.Create();
        #endregion

        #region Methods
        public static byte[] CalculateHash(byte[] value) => SHA256.ComputeHash(value);

        public static byte[] CalculateHash(string value) => CalculateHash(Encoding.Default.GetBytes(value));

        public static string CalculateHashToString(byte[] value) => HashToString(CalculateHash(value));

        public static string CalculateHashToString(string value) => HashToString(CalculateHash(value));

        public static string HashToString(byte[] hash) => Encoding.Default.GetString(hash);
        #endregion
    }
}
