using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class BusDAO
    {
        private static BusDAO instance;

        private BusDAO() { }

        public static BusDAO Instance
        {
            get { if (instance == null) instance = new BusDAO(); return instance; }
            private set => instance = value;
        }

        public List<Tuyen_xe_bus> GetListBus()
        {
            return DataProvider.Instance.db.Tuyen_xe_bus.ToList();
        }

        public void AddNewBus(string IDbus)
        {
            int testID;
            if (IDbus.Length != 4 || !IDbus.StartsWith("B") || !int.TryParse(IDbus.Substring(1), out testID))
                return;
            var newBus = new Tuyen_xe_bus()
            {
                Ma_tuyen_xe_bus = IDbus
            };

            int count = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == IDbus).Count();
            if (count > 0)
            {
                MessageBox.Show("Trùng lặp mã tuyến tàu/xe");
                return;
            }

            DataProvider.Instance.db.Tuyen_tau_xe.Add(new Tuyen_tau_xe() { Ma_tuyen = IDbus });
            DataProvider.Instance.db.Tuyen_xe_bus.Add(newBus);
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteBus(Tuyen_xe_bus selected)
        {
            var temp = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == selected.Ma_tuyen_xe_bus).SingleOrDefault();
            DataProvider.Instance.db.Tuyen_xe_bus.Remove(selected);
            DataProvider.Instance.db.Tuyen_tau_xe.Remove(temp);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
