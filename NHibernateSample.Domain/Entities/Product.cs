using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateSample.Domain.Entities
{
    public class Product
    {
        public virtual int ProductId { get; set; }

        public virtual int Version { get; set; }

        public virtual string Name { get; set; }

        public virtual float Cost { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }
}
