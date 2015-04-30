using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateSample.Domain.Entities
{
    public class CustomerComponent
    {
        public virtual int Id { get; set; }

        public virtual int Version { get; set; }

        public virtual Name Name { get; set; }
    }
}
