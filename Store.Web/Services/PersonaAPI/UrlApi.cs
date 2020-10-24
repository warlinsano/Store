using Store.Web.Models.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Store.Web.Services.PersonaAPI
{
    public class UrlApi
    {
        public static HttpClient inictial()
        {
            string urlPlantillaApi = AppsetttingsConfig.Configuration.GetSection("ConnectionbApi").GetSection("urlPlantillaApi").Value;

            var UrlWebApi = new HttpClient();
            UrlWebApi.BaseAddress = new Uri(urlPlantillaApi);
            UrlWebApi.DefaultRequestHeaders.Clear();
            UrlWebApi.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
            return UrlWebApi;
        }
        static UrlApi()
        {
        }
    }
}