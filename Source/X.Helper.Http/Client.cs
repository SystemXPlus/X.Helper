using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace X.Helper.Http
{
    public partial class Client
    {
        private static readonly HttpClient _HttpClient = new HttpClient();
        private HttpClient _CustomClient = null;
        private HttpClient HttpClient
        {
            get
            {
                if(_CustomClient == null)
                {
                    lock (this)
                    {
                        if(_CustomClient == null)
                        {
                            _CustomClient = _HttpClient;
                        }
                    }
                }
                return _CustomClient;
            }
        }
        private CookieContainer cookieContainer;

        private string URL { get; set; }
        private Enums.HttpMethod Method { get; set; } = Enums.HttpMethod.GET;
        private string Referer { get; set; }
        private string Origin { get; set; }
        private string UserAgent { get; set; } = "X.Helper.Http.Client";
        private string ContentType { get; set; }
        private Encoding Encoding { get; set; } = Encoding.UTF8;


        private TimeSpan TimeOut = TimeSpan.FromSeconds(100.0);

        public Client(string url)
        {
            this.URL = url;
        }
        public Client (Uri uri)
        {
            this.URL = uri.ToString();
        }



    }
}
