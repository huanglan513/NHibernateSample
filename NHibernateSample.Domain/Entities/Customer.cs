using System;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace NHibernateSample.Domain.Entities
{
    public class Customer
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
        //版本控制
        public virtual int Version { get; set; }

        public virtual ISet<Order> Orders { get; set; }
    }
}
