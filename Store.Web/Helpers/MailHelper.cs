using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Store.Web.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            //var from = _configuration["Mail:From"];
            //var smtp = _configuration["Mail:Smtp"];
            //var port = _configuration["Mail:Port"];
            //var password = _configuration["Mail:Password"];

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress(from));
            //message.To.Add(new MailboxAddress(to));
            //message.Subject = subject;
            //var bodyBuilder = new BodyBuilder
            //{
            //    HtmlBody = body
            //};
            //message.Body = bodyBuilder.ToMessageBody();

            //using (var client = new SmtpClient())
            //{
            //    client.Connect(smtp, int.Parse(port), false);
            //    client.Authenticate(from, password);
            //    client.Send(message);
            //    client.Disconnect(true);
            //}
        }

        //LE FALTAN VALIDACIONES
        //NO TIENE VALIDACION DE SI EL E-MAEIL EXISTE O NO....
        //public string ParaCorreoDestino { private get; set; }
        //public string TituloAsunto { private get; set; }
        //public string CuerpoMensaje { private get; set; }
        //----------------------------
        //var ParaCorreoDestino = "warlinsano@gmail.com";
        //var TituloAsunto = "Prueva Coreo de enviado C#";
        //var CuerpoMensaje = "Estoy Probando si Funciona el envio de email desde la app C#";
        //----------------------------
        public string EnviarMail(string ParaCorreoDestino, string TituloAsunto, string CuerpoMensaje)
        {
            var from = _configuration["Mail:From"];        //"the.warlin@hotmail.com";
            var smtpserver = _configuration["Mail:Smtp"];  //"smtp.live.com"
            var port = _configuration["Mail:Port"];         //587
            var password = _configuration["Mail:Password"]; //"*********";

            string msj = "";
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(from);
                //foreach (var item in collection)
                //{
                if (string.IsNullOrEmpty(ParaCorreoDestino.Trim()))
                {
                    return msj = "Falta Llenar el Email del Destinatario";
                }
                else if (string.IsNullOrEmpty(TituloAsunto.Trim()))
                {
                    return msj = "Deve Llenar el Asunto del Email";
                }
                else if (string.IsNullOrEmpty(CuerpoMensaje.Trim()))
                {
                    return msj = "Deve Llenar el Mensaje del Email";
                }
                else
                {
                    message.To.Add(new MailAddress(ParaCorreoDestino));
                }
                //}
                message.Subject = TituloAsunto;
                message.IsBodyHtml = true;
                message.Body = CuerpoMensaje;
                smtp.Port = int.Parse(port);
                smtp.Host = smtpserver;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                return msj = "Error al Enviar el Email: " + ex.ToString();
            }
            return msj = "Se Envio el Correo";
        }
    }
}
