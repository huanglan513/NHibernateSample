using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateSample.Domain.Entities
{
    public class Name
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }
    }
}
