using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Data.Entities
{
    public class ProductImage
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public Guid ImgeId { get; set; }

        public string ImageFullPatch => ImgeId == Guid.Empty
           ? $"http://warlinsano.somee.com/img/noimageg.jpg"
           : $"https://localhost:44396/img/products/{ImgeId}";

    }
}
