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
    public class ImageSource
    {
        private List<string> paths = new List<string>();
        public string[] ImagePaths { get { return paths.ToArray(); } }

        List<Bitmap> bitMaps = new List<Bitmap>();
        public Bitmap[] Bmps { get { return bitMaps.ToArray(); } }


        public ImageSource(string path)
        {
            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetFiles(path);
                paths.AddRange(dirs);
                for (int i = 0; i < paths.Count; i++)
                {
                    try
                    {
                        bitMaps.Add((Bitmap)Image.FromFile(paths[i]));
                    }
                    catch (Exception ex)
                    {
                        Log.logErrorForce("加载图片异常!", ex);
                    }
                }
            }

        }


    }
}
