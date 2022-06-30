
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tool
{
    public class HOCR
    {
        public enum OCRType
        {
            English, Chinese
        }

        public static OCRType OcrType { get; set; } = OCRType.English;

        public static string getOcr()
        {
            //MODI.Document doc = new MODI.Document();
            //doc.Create("dd");
            return "";
        }

        private static string getOcrType(OCRType type)
        {

            if (type == OCRType.Chinese)
            {
                return "chi_sim_vert";
            }
            return "eng";
        }

        //调用tesseract实现OCR识别
        public static string ImageToText(string imgPath, OCRType type = OCRType.English)
        {
            //using (var engine = new TesseractEngine("tessdata", getOcrType(type), EngineMode.Default))
            //{
            //    using (var img = Pix.LoadFromFile(imgPath))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            return page.GetText();
            //        }
            //    }
            //}
            return "";
        }


        public static string ImageToText(Image bmp, OCRType type = OCRType.English)
        {
            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            //    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
            //    Mat simg = Mat.FromStream(ms, ImreadModes.Grayscale);
            //    //Cv2.ImShow("Input Image", simg);
            //    //阈值操作 阈值参数可以用一些可视化工具来调试得到
            //    Mat ThresholdImg = simg.Threshold(10, 200, ThresholdTypes.Binary);
            //    //Cv2.ImShow("minAreaRect", ThresholdImg);
            //    //Cv2.CvtColor(InputArray.Create(ThresholdImg), OutputArray.Create(ThresholdImg),  ColorConversionCodes.RGB2GRAY);
            //    // Cv2.ImShow("Threshold", ThresholdImg);
            //    ThresholdImg.WriteToStream(ms, ".tiff");
            //    return ImageToText(ms.GetBuffer(), type);
            //}
            return "";
        }


        public static string ImageToText(byte[] imgPath, OCRType type = OCRType.English)
        {
            //using (var engine = new TesseractEngine("tessdata", getOcrType(type), EngineMode.Default))
            //{
            //    engine.SetVariable("tessedit_char_whitelist", "0123456789");
            //    using (var img = Pix.LoadTiffFromMemory(imgPath))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            return page.GetText();
            //        }
            //    }
            //}
            return "";
        }

        public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "MHUExwHYSu7KTdiK6VdlN46G";
        // 百度云中开通对应服务应用的 Secret Key
        private static String clientSecret = "Fa5GMQAOumLPDKNAlmdsEhUfmzTunAOM";

        public static String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            BDSessionData bd = WSTools.WsData.Json.FromJSON<BDSessionData>(result);
            return bd.access_token;
        }

        class BDSessionData
        {
            public string access_token { get; set; }
        }

        public static string getNum(string path)
        {
            return numbers(getFileBase64(path));
        }

        // 数字识别
        public static string numbers(string base64)
        {
            string token = getAccessToken();
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/numbers?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            String str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("数字识别:");
            Console.WriteLine(result);
            return result;
        }

        public static string getFileBase64(string fileName)
        {
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }
    }
}
