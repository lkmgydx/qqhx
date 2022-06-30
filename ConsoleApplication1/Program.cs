using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            getNum("d:\\ff");
        }

        public static string getNum(string img)
        {

            //var APP_ID = "20375427";
            var API_KEY = "MHUExwHYSu7KTdiK6VdlN46G";
            var SECRET_KEY = "Fa5GMQAOumLPDKNAlmdsEhUfmzTunAOM";

            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes("图片文件路径");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasic(image);
            // Console.WriteLine(result);
            //        // 如果有可选参数
            //        var options = new Dictionary<string, object>{
            //    {"language_type", "CHN_ENG"},
            //    {"detect_direction", "true"},
            //    {"detect_language", "true"},
            //    {"probability", "true"}
            //};
            //        // 带参数调用通用文字识别, 图片参数为本地图片
            //        result = client.GeneralBasic(image, options);
            Console.WriteLine(result);
            return "";
        }
    }
}
