using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BrassLoon.DataClient.MongoDB
{
    internal static class Hash
    {
        public static string Compute(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            value = Regex.Replace(value.Trim().ToLower(CultureInfo.InvariantCulture), @"\s{2,}", " ", RegexOptions.None, TimeSpan.FromMilliseconds(200));
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                return string.Join(string.Empty, bytes.Select(b => b.ToString("X2", CultureInfo.InvariantCulture)));
            }
        }
    }
}
