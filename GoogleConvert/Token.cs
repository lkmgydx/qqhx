using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleConvert
{
    /// <summary>
    ///  token工具类
    /// </summary>
    class Token
    {
        private static string js = "";
        private static VsaEngine Engine = VsaEngine.CreateEngine();
        static Token()
        {
            Assembly assembly = typeof(Token).Assembly;
            Stream stream = assembly.GetManifestResourceStream("GoogleConvert.Google.js");
            using (BinaryReader br = new BinaryReader(stream))
            {
                var bt = br.ReadBytes((int)stream.Length);
                js = Encoding.UTF8.GetString(bt);
            }
        }

        public static string getGoogleToken(string msg)
        {
            string dyjs = js.Replace("%s", msg);
            var rst = Eval.JScriptEvaluate(dyjs, Engine);
            return rst.ToString();
        }

    }
}
