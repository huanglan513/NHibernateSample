using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernateSample.Domain.Entities;

namespace NHibernateSample.Data.Test
{
    //[TestFixture]
    public class NHibernateSampleFixture
    {
        private Customer _sample;
      //  [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _sample = new Customer();
        }
       // [Test]
        public void GetCustomerByIdTest()
        {
            var tempCutomer = new Customer { FirstName = "李", LastName = "永京" };
         //   _sample.CreateCustomer(tempCutomer);
         //   Customer customer = _sample.GetCustomerById(1);
       //     int customerId = customer.Id;
          //  Assert.AreEqual(1, customerId);
        }
    }
}
