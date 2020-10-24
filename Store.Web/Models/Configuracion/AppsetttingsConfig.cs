using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models.Configuracion
{
    public class AppsetttingsConfig
    {
        public static IConfiguration Configuration { get; set; }
    }
}