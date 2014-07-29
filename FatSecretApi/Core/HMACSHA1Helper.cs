namespace FatSecretApi.Core
{
    using System.Security.Cryptography;

    public static class HMACSHA1Helper
    {
        public static byte[] ComputeHash(byte[] key, byte[] inputContent)
        {
            byte[] hashValue = null;
            using (HMACSHA1 hmac = new HMACSHA1(key))
            {
                hashValue = hmac.ComputeHash(inputContent);
            }
            return hashValue;
        }
    }
}
