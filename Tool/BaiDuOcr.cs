using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSLog;

namespace Tool
{
    /// <summary>
    /// 百度图像识别
    /// </summary>
    public class BaiDuOcr
    {

        public static string getImgText(string path)
        {
            return getImgText(File.ReadAllBytes(path));
        }

        public static string getImgText(Image bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return getImgText(ms.GetBuffer());
            }
        }

        public static string getImgText(byte[] bt)
        {
            try
            {
                var client = new Baidu.Aip.Ocr.Ocr("MHUExwHYSu7KTdiK6VdlN46G", "Fa5GMQAOumLPDKNAlmdsEhUfmzTunAOM");
                // 调用通用文字识别（高精度版），可能会抛出网络等异常，请使用try/catch捕获
                var result = client.GeneralBasic(bt);
                if (result["error_msg"] != null)
                {
                    string err = "返回异常!" + result["words_result"];
                    Console.WriteLine(err);
                    Log.logError("返回异常!" + result.ToString());
                }
                if (result["words_result"].Count() == 0) {
                    return "";
                }
                return result["words_result"][0]["words"].ToString();
            }
            catch (Exception)
            {
            }
            return "";
        }
    }
}
