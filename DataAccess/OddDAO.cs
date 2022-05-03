using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class OddDAO
    {
        private static OddDAO instance;

        private OddDAO() { }

        public static OddDAO Instance
        {
            get { if (instance == null) instance = new OddDAO(); return instance; }
            private set => instance = value;
        }

        public List<Ve_le> GetListOdd()
        {
            return DataProvider.Instance.db.Ve_le.ToList();
        }

        public void UpdateOdd(Ve_le selected, string IDroute, DateTime? date, string IDstop1, string IDstop2, DateTime? come, DateTime? leave)
        {
            if (come != null && leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian khách lên xuống không hợp lý");
                return;
            }

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

            if (date != null)
            {
                var temp2 = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
                if (temp2 != null && temp2.Ngay_gio_mua.Value.Date > date.Value.Date)
                {
                    MessageBox.Show("Ngày sử dụng không hợp lý.");
                    return;
                }
            }

            var upt = DataProvider.Instance.db.Ve_le.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
            upt.Ma_tuyen = IDroute;
            upt.Ngay_su_dung = date;
            upt.Ma_ga_tram_len = IDstop1;
            upt.Ma_ga_tram_xuong = IDstop2;

            if (come != null) upt.Gio_len = come.Value.TimeOfDay;
            if (leave != null) upt.Gio_xuong = leave.Value.TimeOfDay;

            DataProvider.Instance.db.SaveChanges();
        }

        //public void DeleteOdd(Ve_le selected)
        //{
        //    DataProvider.Instance.db.Ve_le.Remove(selected);
        //    DataProvider.Instance.db.SaveChanges();
        //}
    }
}
