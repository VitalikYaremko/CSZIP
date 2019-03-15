using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Helpers
{
    public static class HttpUtils
    {
        public const int DefaultTimeout = 20000;
        public async static Task<object> PostJson(string url, object obj, int timeout = DefaultTimeout)
        {
            return await PostJson<object>(url, obj, timeout);
        }
        public async static Task<T> PostJson<T>(string url, object obj, int timeout = DefaultTimeout, string methodType = "POST")
        {
            try
            {
                HttpWebRequest post = (HttpWebRequest)WebRequest.Create(url);
                post.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                post.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.89 Safari/537.36";
                post.Timeout = timeout;
                post.ContentType = "application/json";
                post.Method = methodType;
                 
                Byte[] bytes = new ASCIIEncoding().GetBytes(JsonConvert.SerializeObject(obj));

                //Set data in request
                Stream requestStream = await post.GetRequestStreamAsync();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                //Get the response 
                using (var response = await post.GetResponseAsync())
                {
                    var responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    if ((responseContent.StartsWith("{") && responseContent.EndsWith("}")) || (responseContent.StartsWith("[") && responseContent.EndsWith("]")))
                    {
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    return (T)Convert.ChangeType(responseContent, typeof(T));
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
