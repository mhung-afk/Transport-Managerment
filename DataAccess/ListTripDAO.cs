using System.Collections.Generic;
using System.Linq;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class ListTripDAO
    {
        private static ListTripDAO instance;

        private ListTripDAO() { }

        public static ListTripDAO Instance
        {
            get { if (instance == null) instance = new ListTripDAO(); return instance; }
            private set => instance = value;
        }

        public List<Chuyen_tau_xe> GetListTripOrder(string IDroute)
        {
            return DataProvider.Instance.db.Chuyen_tau_xe.Where(x => x.Ma_tuyen == IDroute).ToList();
        }
    }
}
