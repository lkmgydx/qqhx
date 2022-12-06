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

    [Serializable]
    public class ImgInfo
    {
        public string Base64ImgStr { get; set; }
        public string OrcText { get; set; }

        public string Md5Key { get; set; }
    }


    public class ImageCacheTool
    {
        private static string OCRNAME = "hocr.bin";
        private static Dictionary<string, ImgInfo> DATA;
        static ImageCacheTool()
        {
            if (File.Exists(OCRNAME))
            {
                DATA = WSTools.WSSerialize.BinSerialize.fromFile(OCRNAME) as Dictionary<string, ImgInfo>;
            }
            if (DATA == null)
            {
                DATA = new Dictionary<string, ImgInfo>();
            }
        }

        static void saveData()
        {
            try
            {
                WSTools.WSSerialize.BinSerialize.toFile(OCRNAME, DATA);
            }
            catch (Exception ex)
            {
                Log.logError("文字识别序列化失败！", ex);
            }
        }

        public static string getOrcText(Bitmap bmp)
        {
            string key = ImageTool.UUIDImg(bmp);
            if (DATA.ContainsKey(key))
            {
                return DATA[key].OrcText;
            }
            else
            {
                string tex = BaiDuOcr.getImgText(bmp);
                if (string.IsNullOrEmpty(tex))
                {
                    tex = "识别异常";
                }
                else
                {
                    string msg = "来自百度自动识别结果:[" + key + ":" + tex + "]";
                    Console.WriteLine(msg);
                    WSTools.WSLog.Log.logForce(msg);
                }
                ImgInfo ii = new ImgInfo();
                ii.OrcText = tex;
                ii.Md5Key = key;
                ii.Base64ImgStr = ImageTool.getImgBase64(bmp);
                DATA.Add(ii.Md5Key, ii);
                saveData();
                return tex;
            }
        }

        public static string getOrcText(string key)
        {
            if (DATA.ContainsKey(key))
            {
                return DATA[key].OrcText;
            }
            return null;
        }
    }
}
