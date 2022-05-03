using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class MonthDAO
    {
        private static MonthDAO instance;

        private MonthDAO() { }

        public static MonthDAO Instance
        {
            get { if (instance == null) instance = new MonthDAO(); return instance; }
            private set => instance = value;
        }

        public List<Ve_thang> GetListMonth()
        {
            return DataProvider.Instance.db.Ve_thang.ToList();
        }

        public List<Ga_Tram> GetStopByIdTicket(string ID)
        {
            var temp = DataProvider.Instance.db.Ve_thang.Where(x => x.Ma_ve == ID).SingleOrDefault();
            if (temp != null)
            {
                var t1 = StopDAO.Instance.GetStop(temp.Ma_ga_tram_1);
                var t2 = StopDAO.Instance.GetStop(temp.Ma_ga_tram_2);
                return new List<Ga_Tram>() { t1, t2 };
            }
            return null;
        }

        public void UpdateMonth(Ve_thang selected, string IDroute, string IDstop1, string IDstop2)
        {
            var temp = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop1).Count();
            if (temp == 0)
            {
                MessageBox.Show("Ga/Trạm lên không thuộc tuyến" + IDroute);
                return;
            }

            var temp1 = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop2).Count();
            if (temp1 == 0)
            {
                MessageBox.Show("Ga/Trạm xuống không thuộc tuyến" + IDroute);
                return;
            }

            var upt = DataProvider.Instance.db.Ve_thang.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
            upt.Ma_tuyen = IDroute;
            upt.Ma_ga_tram_1 = IDstop1;
            upt.Ma_ga_tram_2 = IDstop2;

            DataProvider.Instance.db.SaveChanges();
        }
    }
}
