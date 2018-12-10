
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Agile.Core
{
    /// <summary>
    /// http请求工具
    /// </summary>
    public class HttpHelper
    {
        private static readonly object SyncObj = new object();

        private static HttpHelper _instance;

        public static HttpHelper Instance
        {
            get
            {
                if (HttpHelper._instance == null)
                {
                    lock (HttpHelper.SyncObj)
                    {
                        if (HttpHelper._instance == null)
                        {
                            HttpHelper._instance = new HttpHelper();
                        }
                    }
                }
                return HttpHelper._instance;
            }
        }


        private HttpHelper()
        {

        }


        public enum PostContentType
        {
            Json,
            Xml,
            Form
        }




        /// <summary>
        /// http Get请求
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        /// <param name="headers">请求header信息</param>
        /// <param name="timeout">超时时间1000*90</param>
        /// <returns></returns>
        public string HttpGet(string requestUrl, Dictionary<string, string> headers = null, int timeout = 90000)
        {
            if (string.IsNullOrEmpty(requestUrl))
            {
                return "";
            }

            HttpWebRequest webRequest = (WebRequest.Create(requestUrl) as HttpWebRequest);
            if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = HttpHelper.CheckValidationResult;
                webRequest.ProtocolVersion = HttpVersion.Version10;
            }

            webRequest.Method = "GET";
            webRequest.Timeout = timeout;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value);
                }
            }

            string result = string.Empty;
            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                //如果webResponse.StatusCode的值为HttpStatusCode.OK，表示成功
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// http Post请求
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        /// <param name="objPostData">发送数据,form表单格式传字典，json格式传json文本，xml格式传xml文本</param>
        /// <param name="contentType">发送数据类型</param>
        /// <param name="headers">请求header信息</param>
        /// <param name="timeout">超时时间1000*90</param>
        /// <returns></returns>
        public string HttpPost(string requestUrl, object objPostData, PostContentType contentType, Dictionary<string, string> headers = null, int timeout = 90000)
        {
            if (string.IsNullOrEmpty(requestUrl))
            {
                return "";
            }

            HttpWebRequest webRequest = (WebRequest.Create(requestUrl) as HttpWebRequest);
            if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = HttpHelper.CheckValidationResult;
                webRequest.ProtocolVersion = HttpVersion.Version10;
            }
            webRequest.Method = "POST";
            webRequest.Timeout = timeout;

            //表单数据
            if (contentType == PostContentType.Form)
            {
                Dictionary<string, string> parameters = objPostData as Dictionary<string, string>;
                if (!(parameters == null || parameters.Count == 0))
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(parameters[key], Encoding.UTF8));
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(parameters[key], Encoding.UTF8));
                        }
                        i++;
                    }
                    byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                    webRequest.ContentLength = data.Length;
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            else if (contentType == PostContentType.Json)//json数据
            {
                string jsonData = objPostData as string;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    webRequest.ContentType = "application/json";
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    webRequest.ContentLength = data.Length;
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            else if (contentType == PostContentType.Xml)//xml数据
            {
                string jsonData = objPostData as string;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    webRequest.ContentType = "application/xml";
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    webRequest.ContentLength = data.Length;
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value);
                }
            }

            string result = String.Empty;
            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                //如果webResponse.StatusCode的值为HttpStatusCode.OK，表示成功
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }

            return result;
        }




        public string ToMD5(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
            {
                return true;
            }
            return false;
        }
    }
}
