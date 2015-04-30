using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateSample.Data
{
    public class NHibernateHelper
    {
        private ISessionFactory sessionFactory;

        public NHibernateHelper()
        {
            sessionFactory = GetSessionFactory();
        }

        private ISessionFactory GetSessionFactory()
        {
            return (new Configuration()).Configure().BuildSessionFactory();
        }

        public ISession GetSession()
        {
            return sessionFactory.OpenSession();
        }
    }
}
