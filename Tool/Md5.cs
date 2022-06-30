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
            return getMd5(System.Text.Encoding.Default.GetBytes(str));
        }

        public static string getMd5(byte[] bt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(bt);
            return BitConverter.ToString(result).Replace("-","");
        }
    }
}
