namespace WTLib.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class PasswordUtil
    {
        /// <summary>
        /// 将密码进行SHA256和MD5Hash后拼接放入数据库
        /// </summary>
        public static string Encrypt(string password)
        {
            SHA256 sha256Managed = new SHA256Managed();
            byte[] sha256Array = sha256Managed.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(password));
            StringBuilder sbSha256 = new StringBuilder(32);
            for (int i = 0; i < sha256Array.Length; i++)
            {
                sbSha256.Append(sha256Array[i].ToString("x").PadLeft(2, '0'));
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Array = md5.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(sbSha256.ToString()));
            StringBuilder sbMd5 = new StringBuilder(md5Array.Length);
            for (int i = 0; i < md5Array.Length; i++)
            {
                sbMd5.Append(md5Array[i].ToString("x").PadLeft(2, '0'));
            }

            return sbMd5.ToString();
        }

        public static string ToBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var bytes = Encoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static string DecryptBase64(string base64Password)
        {
            if (base64Password == null)
                return null;

            var bytes = Convert.FromBase64String(base64Password);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
