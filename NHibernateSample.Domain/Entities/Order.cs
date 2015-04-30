using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateSample.Domain.Entities
{
    public class Order
    {
        public virtual int OrderId { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual int Version { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
