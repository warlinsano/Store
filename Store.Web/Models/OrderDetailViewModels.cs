using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Models
{
    public class OrderDetailViewModels
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public short Cantidad { get; set; } //TODO: cambiar a int
        public decimal PrecioUnitario { get; set; }
        public bool Retirar { get; set; }
        public decimal Monto()
        {
            return Cantidad * PrecioUnitario;
        }
    }
}
