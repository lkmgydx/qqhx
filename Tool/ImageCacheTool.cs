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
    public class ImageCacheTool
    { 
        public static List<string> getList(string rootPath)
        {
            List<string> keys = new List<string>();
            if (Directory.Exists(rootPath))
            {
                string[] dirs = Directory.GetFiles(rootPath);
                foreach (var file in dirs)
                {
                    try
                    {
                        string key = getBaseKey((Bitmap)Image.FromFile(file));
                        if (!keys.Contains(key))
                        {
                            keys.Add(key);
                            //File.Copy(file, @"D:\qqhximg\some\v\" +Path.GetFileName(file));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.logError("加载图片列表!" + file, ex);
                    }
                }
            }
            return keys;
        }

        public static string getBaseKey(Bitmap img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return Convert.ToBase64String(ms.GetBuffer());
            }
        }
    }
}
