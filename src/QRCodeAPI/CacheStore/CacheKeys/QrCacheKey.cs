using System.Security.Cryptography;
using System.Text;

namespace QRCodeAPI.CacheStore.CacheKeys
{
    public class QrCacheKey : ICacheKey
    {
        public string Key { get; private set; }

        public QrCacheKey(string key)
        {
            using (var md5Hasher = MD5.Create())
            {
                Key = GetMd5Hash(md5Hasher, key);
            }
        }

        public override string ToString()
        {
            return $"QrCacheKey('{Key}')";
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}