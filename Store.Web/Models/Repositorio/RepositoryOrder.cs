using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models.Repositorio
{
    //public class RepositoryOrder : IRepository<Solicitud>
    //{
    //    public IEnumerable<Solicitud> GetAll()
    //    {
    //        var response = new HttpResponseMessage();
    //        var model = new List<Solicitud>();
    //        response = UrlApi.inictial().GetAsync("Solicituds").Result;
    //        if (response.IsSuccessStatusCode)
    //        {
    //            var Lista = response.Content.ReadAsStringAsync().Result;
    //            model = (JsonConvert.DeserializeObject<List<Solicitud>>(Lista));
    //        }
    //        return model;
    //    }

    //    public Solicitud GetById(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return null;
    //        }

    //        var response = new HttpResponseMessage();
    //        response = UrlApi.inictial().GetAsync("Solicituds/" + id + "").Result;
    //        var model = new Solicitud();
    //        if (response.IsSuccessStatusCode)
    //        {
    //            var SolicitudSerel = response.Content.ReadAsStringAsync().Result;
    //            return JsonConvert.DeserializeObject<Solicitud>(SolicitudSerel);
    //        }
    //        return null;
    //    }

    //    public Solicitud Add(Solicitud entity)
    //    {
    //        try
    //        {
    //            var response = new HttpResponseMessage();
    //            var requestString = JsonConvert.SerializeObject(entity);
    //            var content = new StringContent(requestString, Encoding.UTF8, "application/json");
    //            response = UrlApi.inictial().PostAsync("Solicituds", content).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var result = response.Content.ReadAsStringAsync().Result;
    //                return JsonConvert.DeserializeObject<Solicitud>(result);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }
    //        return null;
    //    }

    //    public Solicitud Remove(int? id)
    //    {
    //        try
    //        {
    //            var response = new HttpResponseMessage();
    //            response = UrlApi.inictial().DeleteAsync("Solicituds/ " + id + "").Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var result = response.Content.ReadAsStringAsync().Result;
    //                return JsonConvert.DeserializeObject<Solicitud>(result);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }
    //        return null;
    //    }

    //    public Solicitud Update(int id, Solicitud entity)
    //    {
    //        try
    //        {
    //            var response = new HttpResponseMessage();
    //            var requestString = JsonConvert.SerializeObject(entity);
    //            var content = new StringContent(requestString, Encoding.UTF8, "application/json");
    //            response = UrlApi.inictial().PutAsync("Solicituds/ " + id + "", content).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var result = response.Content.ReadAsStringAsync().Result;
    //                return JsonConvert.DeserializeObject<Solicitud>(result);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }
    //        return null;
    //    }

    //    public bool Exists(int id)
    //    {
    //        //return _context.Solicitud.Any(e => e.IdSolicitud == id);
    //        return true;
    //    }
    //}
}