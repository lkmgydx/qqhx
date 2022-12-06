using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class Md5
    {
        public static string getMd5(string str)
        {
            return getMd5(Encoding.UTF8.GetBytes(str));
        }
        private static MD5 md5 = new MD5CryptoServiceProvider();
        public static string getMd5(byte[] bt)
        {
            lock (md5)
            {
                byte[] result = md5.ComputeHash(bt);
                return BitConverter.ToString(result).Replace("-", "");
            }
        }
    }
}
