using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using ViewModels;

namespace RestApiTests
{
    class Client
    {
        //HttpCookie LoginCookie = new HttpCookie(".ASPXAUTH");
        public string EndPoint { get; set; }

        public string Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public static string LoginCookieValue { get; set; }

        public Client(string url = "", string domain = "")
        {
            if (url == "")
                url = "https://localhost:44380/";
                //url = ConfigurationManager.AppSettings.GetValues("Name");

            EndPoint = url;
            if (domain != "")
                Domain = domain;
            else
                Domain = ConfigurationManager.AppSettings["Domain"];
            //Method = methodtype;
            ContentType = "application/json";
            PostData = "";
        }

        public string LoginRequest(UserCredentials userCredentials,string controllerUrl, string methodType)
        {
            Method = methodType;
            var postData = "grant_type=" + Uri.EscapeDataString(userCredentials.grant_type);
            postData += "&username=" + Uri.EscapeDataString(userCredentials.Name);
            postData += "&password=" + Uri.EscapeDataString(userCredentials.Password);
            return Request(controllerUrl, postData, Method, false, true);
        }

        public string Request(string controllerUrl, string paramString, string methodType, bool getCookie = true, bool setCookie = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + controllerUrl);
            var data = Encoding.ASCII.GetBytes(paramString);

            request.Method = methodType;
            // request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = ContentType;
            request.ContentLength = data.Length;

            // Add Post and Put object into request
            if (request.Method == "POST" || request.Method == "PUT" || request.Method == "DELETE")
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            try
            {
                // Parse Response
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Faile: Received HTTP {0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }
                    return responseValue;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Extract ", e.Message);
                return e.Message;
            }
        }
    }
}
