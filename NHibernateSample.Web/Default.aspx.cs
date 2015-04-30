using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernateSample.Data;
using NHibernateSample.Domain.Entities;

namespace NHibernateSample.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NHibernateHelper helper = new NHibernateHelper();

            var tempCustomer = new Customer { FirstName = "李", LastName = "永" };

            CustomerData customerData = new CustomerData(helper.GetSession());
            //customerData.CreateCustomer(tempCustomer);
            //Customer customer = customerData.GetCustomerById(1);
            //int customerId = customer.Id;
            //lblText.Text = customerId.ToString();

            IList<CustomerComponent> customerComList = customerData.ReturnFullName("ss", "dd");
            Label1.Text = customerComList[0].Name.FullName;
            //IList<Customer> customerList=customerData.From();
            //Label1.Text = customerList[0].Id.ToString();

            DateTime dt = new DateTime(2012, 1, 1);
           // IList<Customer> customerList = customerData.UseSQL_GetCustomersWithOrders(dt);
          //  IList<Customer> customerList = customerData.UseHQL_GetCustomersWithOrders(dt);
          //  IList<Customer> customerList = customerData.UseCriterialAPI_GetCustomersWithOrders(dt);
         //   IList<Customer> customerList = customerData.UseCriterialAPI_GetDistinctCustomers(dt);
         //   IList<Customer> customerList = customerData.UseSQL_GetCustomersWithOrdersHavingProduct(dt);
         //   IList<Customer> customerList = customerData.UseHQL_GetCustomersWithOrdersHavingProduct(dt);
            IList<Customer> customerList = customerData.UseCriteriaAPI_GetCustomersWithOrdersHavingProduct(dt);
            lblText.Text = customerList[0].Id.ToString();

            Customer customer = customerData.LazyLoad(1);

            Customer customer2 = customerData.LazyLoadUsingSession(1);

            Order order = customerData.LazyLoadOrderAggregate(1);

            Order order2 = customerData.LazyLoadOrderAggregateUsingSession(1);

            Customer customer3 = customerData.EagerLoadUsingSessionAndNHibernateUtil(1);

            IList<CustomerView> customerViewList = customerData.GetCustomerView(dt);
        }
    }
}
