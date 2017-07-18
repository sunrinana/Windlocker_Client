﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPFTest
{
    class Server
    {
        private static readonly string SERVER_URL = "http://iwin247.kr:3002";
        public static string GET(string URL, string Content)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL+Content);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(response.StatusCode);
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (WebException we)
            {
                //Console.WriteLine(((HttpWebResponse)we.Response).StatusCode);
                return "";
            }
        }

        public static string POST(string URL, string Content)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL);
            var postData = Content;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Method = "POST";
            var data = Encoding.ASCII.GetBytes(postData);
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (WebException we)
            {
                Console.WriteLine(((HttpWebResponse)we.Response).StatusCode);
                return "";
            }
        }

        public static bool Signup(string id,string pw,string name)
        {
            string post = POST(SERVER_URL + "/auth/signup", "id=" + id + "&passwd=" + pw + "&name=" + name);
            if (post != "")
            {
                JObject j = JObject.Parse(post);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Login(string id, string pw)
        {
            string post = POST(SERVER_URL + "/auth/signin", "id=" + id + "&passwd=" + pw);
            if(post!="")
            {
                JObject j = JObject.Parse(post);
                JToken jt = j["token"];
                return jt.ToString();
            }
            else
            {
                return "error";
            }
        }

        public static bool Auto(string token)
        {
            string get = GET(SERVER_URL,"/" + token);
            if (get != "")
                return true;
            else
                return false;
        }

        public static bool IsLock(string token)
        {
            string get = GET(SERVER_URL, "/" + token);
            if (get != "")
                return true;
            else
                return false;
        }
    }
}