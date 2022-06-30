using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSLog;

namespace Tool
{
    /// <summary>
    /// 本地图像转文字转换工具
    /// </summary>
    public sealed class HYLOcr
    {

        public static string getRst(string file)
        {
            return getRst(System.IO.File.ReadAllBytes(file));
        }

        public static string getRst(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                string temp = Convert.ToBase64String(ms.GetBuffer());
                temp.ToString();
                return getRst(ms.GetBuffer());
            }
        }

        public static string getRst(byte[] btf)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
                System.Net.Sockets.TcpClient tc = new System.Net.Sockets.TcpClient();
                tc.Connect(ip);
                NetworkStream ns = tc.GetStream();
                ns.Write(btf, 0, btf.Length);
                byte[] buffer = new byte[tc.Available];
                ns.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                Log.logError("本地转换失败!", ex);
            }
            return "";
        }

    }
}
