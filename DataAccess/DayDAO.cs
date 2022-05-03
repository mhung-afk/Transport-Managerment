using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class DayDAO
    {
        private static DayDAO instance;

        private DayDAO() { }

        public static DayDAO Instance
        {
            get { if (instance == null) instance = new DayDAO(); return instance; }
            private set => instance = value;
        }

        public List<Ve_1_ngay> GetListDay()
        {
            return DataProvider.Instance.db.Ve_1_ngay.ToList();
        }

        public void UpdateDay(Ve_1_ngay selected, DateTime? date)
        {
            if (date != null)
            {
                var temp = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
                if (temp != null && temp.Ngay_gio_mua.Value.Date > date.Value.Date)
                {
                    MessageBox.Show("Ngày sử dụng không hợp lý.");
                    return;
                }
            }

            var upt = DataProvider.Instance.db.Ve_1_ngay.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
            upt.Ngay_su_dung = date;
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
