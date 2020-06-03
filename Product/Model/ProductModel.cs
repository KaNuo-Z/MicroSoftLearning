using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Model
{
    public class ProductModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Discount { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime OverTime { get; set; }
    }
}
