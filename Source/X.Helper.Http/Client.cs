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
        private readonly HttpClient _HttpClient;

        private CookieContainer cookieContainer;

        private Uri Uri { get; set; }
        private Enums.HttpMethod Method { get; set; } = Enums.HttpMethod.GET;
        private string Referer { get; set; }
        private string Origin { get; set; }
        private string UserAgent { get; set; } = "X.Helper.Http.Client";
        private string ContentType { get; set; }
        private Encoding Encoding { get; set; } = Encoding.UTF8;
        private IPEndPoint IPEndPoint { get; set; } = null;


        private TimeSpan TimeOut = TimeSpan.FromSeconds(100.0);


        public Client():this(handler: null)
        {
            
        }

        public Client(HttpMessageHandler handler)
        {
            if (handler != null)
            {
                _HttpClient = new HttpClient(handler);
            }
            else
            {
                _HttpClient = new HttpClient();
            }
        }


        #region FUNCTION

        public string Get

        #endregion


    }
}
