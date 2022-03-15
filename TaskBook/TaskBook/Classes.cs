using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TaskBook
{
    public static class Encrypt
    {
      
        public static string Sha256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}