using System.Collections.Generic;
using System.Linq;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class RouteDAO
    {
        private static RouteDAO instance;

        private RouteDAO() { }

        public static RouteDAO Instance
        {
            get { if (instance == null) instance = new RouteDAO(); return instance; }
            private set => instance = value;
        }

        public Tuyen_tau_xe GetRoute(string IDroute)
        {
            return DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == IDroute).SingleOrDefault();
        }

        public List<Tuyen_tau_xe> GetListRoute()
        {
            return DataProvider.Instance.db.Tuyen_tau_xe.ToList();
        }
    }
}
