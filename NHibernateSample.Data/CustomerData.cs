using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernateSample.Domain.Entities;
using NHibernate.Criterion;

namespace NHibernateSample.Data
{
    public class CustomerData
    {
        protected ISession Session { get; set; }

        public CustomerData(ISession session)
        {
            Session = session;
        }

        public int CreateCustomer(Customer customer)
        {
            int newid = (int)Session.Save(customer);
            Session.Flush();
            return newid;
        }

        public void DeleteCustomer(Customer customer)
        {
            Session.Delete(customer);
            Session.Flush();
        }

        public void UpdateCustomer(Customer customer)
        {
            Session.Update(customer);
            Session.Flush();
        }

        public void SaveOrUpdateCustomer(IList<Customer> customerList)
        {
            foreach (var c in customerList)
            {
                Session.SaveOrUpdate(c);
            }
            Session.Flush();
        }

        public int CreateCustomerTransaction(Customer customer)
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                try
                {
                    int newId = (int)Session.Save(customer);
                    Session.Flush();
                    tx.Commit();
                    return newId;
                }
                catch (HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void DeleteCustomerTransaction(Customer customer)
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                try
                {
                    Session.Delete(customer);
                    Session.Flush();
                    tx.Commit();
                }
                catch (HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void UpdateCustomerTransaction(Customer customer)
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                try
                {
                    Session.Update(customer);
                    Session.Flush();
                    tx.Commit();
                }
                catch (HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void SaveOrUpdateCustomersTransaction(IList<Customer> customerList)
        {
            using (ITransaction tx = Session.BeginTransaction())
            {
                try
                {
                    foreach (Customer c in customerList)
                        Session.SaveOrUpdate(c);
                    Session.Flush();
                    tx.Commit();
                }
                catch (HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            return Session.Get<Customer>(customerId);
        }

        public IList<Customer> From()
        {
            return Session.CreateQuery("from Customer").List<Customer>();
        }

        public IList<Customer> FromAlias()
        {
            return Session.CreateQuery("from Customer as customer").List<Customer>();
        }

        public IList<int> Select()
        {
            return Session.CreateQuery("select c.CustomerId from Customer c").List<int>();
        }

        public IList<object[]> SelectObject()
        {
            return Session.CreateQuery("select c.FirstName,count(c.FirstName) from Customer c group by c.FirstName").List<Object[]>();
        }

        public IList<object[]> AggregateFunction()
        {
            return Session.CreateQuery("select avg(c.CustomerId),sum(c.CustomerId),count(c) from Customer c")
                .List<object[]>();
        }

        public IList<Customer> Where()
        {
            return Session.CreateQuery("from Customer c where c.Firstname='YJing'")
                .List<Customer>();
        }

        public IList<Customer> Orderby()
        {
            return Session.CreateQuery("from Customer c order by c.Firstname asc,c.Lastname desc")
                .List<Customer>();
        }

        public IList<object[]> Groupby()
        {
            return Session.CreateQuery("select c.Firstname, count(c.Firstname) from Customer c group by c.Firstname")
                .List<object[]>();
        }

        public IList<Customer> CreateCriteria()
        {
            ICriteria crit = Session.CreateCriteria(typeof(Customer));
            crit.SetMaxResults(50);
            IList<Customer> customers = crit.List<Customer>();
            return customers;
        }

        public IList<Customer> Narrowing()
        {
            IList<Customer> customers = Session.CreateCriteria(typeof(Customer)).Add(Restrictions.Like("FirstName", "YJing%"))
                                        .Add(Restrictions.Between("LastName", "A%", "Y%")).List<Customer>();
            return customers;
        }

        public IList<Customer> Order()
        {
            return Session.CreateCriteria(typeof(Customer))
                    .Add(Restrictions.Like("FirstName", "Y%"))
                    .AddOrder(new NHibernate.Criterion.Order("FirstName", false))
                    .AddOrder(new NHibernate.Criterion.Order("LastName", true))
                    .List<Customer>();
        }


        public IList<Customer> Query()
        {
            Customer customerSample = new Customer() { FirstName = "Li", LastName = "Yong" };
            return Session.CreateCriteria(typeof(Customer))
                          .Add(Example.Create(customerSample))
                          .List<Customer>();
        }


        public IList<Customer> GetCustomersByFirstNameAndLastName(string firstname, string lastname)
        {
            return Session.CreateCriteria(typeof(Customer))
                    .Add(Restrictions.Eq("FirstName", firstname))
                    .Add(Restrictions.Eq("LastName", lastname))
                    .List<Customer>();
        }

        public IList<Customer> GetCustomersWithIdGreaterThan(int customerId)
        {
            return Session.CreateCriteria(typeof(Customer))
                    .Add(Restrictions.Gt("CustomerId", customerId))
                    .List<Customer>();
        }

        public IList<CustomerComponent> ReturnFullName(string firstName, string lastName)
        {
            return Session.CreateQuery("from CustomerComponent c where c.Name.FirstName=:fn and c.Name.LastName=:ln")
                    .SetString("fn", firstName)
                    .SetString("ln", lastName)
                    .List<CustomerComponent>();
        }

        public IList<Customer> ReturnFullNameCustomer(string firstName, string lastName)
        {
            return Session.CreateQuery("from Customer c where c.FirstName=:fn and c.LastName=:ln")
                    .SetString("fn", firstName)
                    .SetString("ln", lastName)
                    .List<Customer>();
        }

        public IList<Customer> UseSQL_GetCustomersWithOrders(DateTime orderDate)
        {
            return Session.CreateSQLQuery("select distinct {customer.*} from Customer {customer}" +
                " inner join [Order] o on o.CustomerId={customer}.CustomerId where o.OrderDate>:orderDate")
                .AddEntity("customer", typeof(Customer))
                .SetDateTime("orderDate", orderDate)
                .List<Customer>();
        }

        public IList<Customer> UseHQL_GetCustomersWithOrders(DateTime orderDate)
        {
            return Session.CreateQuery("select distinct c from Customer c inner join c.Orders o  where o.OrderDate > :orderDate")
                         .SetDateTime("orderDate", orderDate)
                         .List<Customer>();
        }

        public IList<Customer> UseCriterialAPI_GetCustomersWithOrders(DateTime orderDate)
        {
            return Session.CreateCriteria(typeof(Customer))
                .CreateCriteria("Orders")
                .Add(Restrictions.Gt("OrderDate", orderDate))
                .SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer())
                .List<Customer>();
        }

        public IList<Customer> UseCriterialAPI_GetDistinctCustomers(DateTime orderDate)
        {
            IList<int> ids = Session.CreateCriteria(typeof(Customer))
                            .SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Property("Id"))))
                            .CreateCriteria("Orders")
                            .Add(Restrictions.Gt("OrderDate", orderDate))
                            .List<int>();

            return Session.CreateCriteria(typeof(Customer))
                            .Add(Restrictions.In("Id", ids.ToArray<int>()))
                            .List<Customer>();
        }

        public IList<Customer> UseSQL_GetCustomersWithOrdersHavingProduct(DateTime orderDate)
        {
            return Session.CreateSQLQuery("select distinct {customer.*} from Customer {customer}" +
                " inner join [Order] o on o.CustomerId={customer}.CustomerId" +
                " inner join OrderProduct op on o.OrderId=op.OrderId" +
                " inner join Product p on op.ProductId=p.ProductId where o.OrderDate>:orderDate")
                .AddEntity("customer", typeof(Customer))
                .SetDateTime("orderDate", orderDate)
                .List<Customer>();
        }

        public IList<Customer> UseHQL_GetCustomersWithOrdersHavingProduct(DateTime orderDate)
        {
            return Session.CreateQuery("select distinct c from Customer c inner join "
                + " c.Orders  o where o.OrderDate>:orderDate")
                .SetDateTime("orderDate", orderDate)
                .List<Customer>();
        }

        public IList<Customer> UseCriteriaAPI_GetCustomersWithOrdersHavingProduct(DateTime orderDate)
        {
            return Session.CreateCriteria(typeof(Customer))
                .Add(Restrictions.Eq("FirstName", "ss"))
                .CreateCriteria("Orders")
                .Add(Restrictions.Gt("OrderDate", orderDate))
                .CreateCriteria("Products")
                .Add(Restrictions.Eq("Name", "Book1"))
                .List<Customer>();
        }

        public Customer LazyLoad(int customerId)
        {
            return Session.Get<Customer>(customerId);
        }

        public Customer LazyLoadUsingSession(int customerId)
        {
            using (ISession session = new NHibernateHelper().GetSession())
            {
                return session.Get<Customer>(customerId);
            }
        }

        public NHibernateSample.Domain.Entities.Order LazyLoadOrderAggregate(int orderId)
        {
            return Session.Get<NHibernateSample.Domain.Entities.Order>(orderId);
        }

        public NHibernateSample.Domain.Entities.Order LazyLoadOrderAggregateUsingSession(int orderId)
        {
            using (ISession session = new NHibernateHelper().GetSession())
            {
                NHibernateSample.Domain.Entities.Order order= session.Get<NHibernateSample.Domain.Entities.Order>(orderId);
                return order;
            }
        }

        public Customer EagerLoadUsingSessionAndNHibernateUtil(int customerId)
        {
            using (ISession session = new NHibernateHelper().GetSession())
            {
                Customer customer = session.Get<Customer>(customerId);
                NHibernateUtil.Initialize(customer.Orders);
                return customer;
            }
        }

        public NHibernateSample.Domain.Entities.Order EagerLoadOrderAggregateWithHQL(int orderId)
        {
            using (ISession _session = new NHibernateHelper().GetSession())
            {
                return _session.CreateQuery("from Order o" +
                    " left outer join fetch o.Products" +
                    " inner join fetch o.Customer where o.OrderId=:orderId")
                    .SetInt32("orderId", orderId)
                    .UniqueResult<NHibernateSample.Domain.Entities.Order>();
            }
        }

        public IList<CustomerView> GetCustomerView(DateTime orderDate)
        {
            return Session.CreateCriteria(typeof(CustomerView))
                .Add(Restrictions.Gt("OrderDate", orderDate))
                .List<CustomerView>();
        }
    }
}
