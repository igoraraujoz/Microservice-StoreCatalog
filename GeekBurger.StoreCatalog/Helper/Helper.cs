using System;
using System.Net.Http;

namespace GeekBurger.StoreCatalog.Helper
{

    public class ProductApi
    {
        public HttpClient Client()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("");
            return client;
        }
    }

    public class ProductionApi
    {
        public HttpClient Client()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("");
            return client;
        }
    }

}
