using System.Security.Cryptography;
using System.Text;

namespace FundraisingApp.Helpers
{
    public static class HashHelper
    {
        public static string GetHash(string key)
        {
            var bytes = ASCIIEncoding.ASCII.GetBytes(key);

            var tmpHash = new MD5CryptoServiceProvider().ComputeHash(bytes);

            return ByteArrayToString(tmpHash);
        }
        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
