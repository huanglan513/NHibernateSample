﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateSample.Domain.Entities
{
    public class CustomerView
    {
        public virtual int CustomerId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int OrderId { get; set; }
        public virtual DateTime OrderDate { get; set; }
    }
}
