using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Helpers
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);

        string EnviarMail(string ParaCorreoDestino, string TituloAsunto, string CuerpoMensaje);
    }
}