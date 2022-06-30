using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace GoogleConvert
{
    public class GoogleCNToEnglish
    {
        public string getUrl(string text)
        {
            return string.Format("https://translate.google.cn/translate_a/single?hl=zh-CN&source=btn&dt=t&tsel=0&ssel=0&q={0}&oe=UTF-8&tk={1}&tl=en&kc=0&client=t&sl=zh-CN&ie=UTF-8", text, Token.getGoogleToken(text));
        }
        public async System.Threading.Tasks.Task<string> getRest(string text)
        {
            HttpClient ch = new HttpClient();
            var f = await ch.GetAsync(getUrl(text));
            string rst = await f.Content.ReadAsStringAsync();
            JArray obj = (JArray)JsonConvert.DeserializeObject(rst);
            return obj[0][0][0].ToString();
        }
    }
}
