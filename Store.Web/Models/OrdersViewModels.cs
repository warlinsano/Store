using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models
{
    public class OrdersViewModels
    {
        #region Cabecera
        public string Cliente { get; set; }
        public string Clienteid { get; set; }
        public int CabeceraProductoId { get; set; }
        public string CabeceraProductoNombre { get; set; }
        public short CabeceraProductoCantidad { get; set; } // convertir en int
        public decimal CabeceraProductoPrecio { get; set; }
        #endregion

        #region Contenido
        public List<OrderDetailViewModels> ComprobanteDetalle { get; set; }
        #endregion

        #region Pie
        public decimal Total()
        {
            return ComprobanteDetalle.Sum(x => x.Monto());
        }
        public DateTime Creado { get; set; }
        #endregion

        public OrdersViewModels()
        {
            ComprobanteDetalle = new List<OrderDetailViewModels>();
            //Refrescar();
            CabeceraProductoId = 0;
            CabeceraProductoNombre = null;
            CabeceraProductoCantidad = 1;
            CabeceraProductoPrecio = 0;
        }
    }
}
