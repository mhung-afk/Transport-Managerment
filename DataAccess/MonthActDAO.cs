using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class MonthActDAO
    {
        private static MonthActDAO instance;

        private MonthActDAO() { }

        public static MonthActDAO Instance
        {
            get { if (instance == null) instance = new MonthActDAO(); return instance; }
            private set => instance = value;
        }

        public List<HD_ve_thang> GetListMonthAct()
        {
            return DataProvider.Instance.db.HD_ve_thang.ToList();
        }

        public void AddMonthAct(string ID, DateTime date, string IDstop1, string IDstop2, DateTime come, DateTime? leave)
        {
            var temp1 = DataProvider.Instance.db.HD_ve_thang.Where(x => x.Ma_ve == ID && x.Ngay_su_dung == date && x.Gio_len == come.TimeOfDay).Count();
            if (temp1 > 0)
            {
                MessageBox.Show("Record đã tồn tại.");
                return;
            }

            if (leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian khách lên xuống không hợp lý");
                return;
            }

            var temp = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve == ID).SingleOrDefault();
            if (temp != null && temp.Ngay_gio_mua.Value.Date > date.Date)
            {
                MessageBox.Show("Ngày sử dụng không hợp lý.");
                return;
            }

            var newAct = new HD_ve_thang()
            {
                Ma_ve = ID,
                Ngay_su_dung = date,
                Ga_tram_len = IDstop1,
                Ga_tram_xuong = IDstop2,
                Gio_len = come.TimeOfDay
            };
            if (leave != null) newAct.Gio_xuong = leave.Value.TimeOfDay;

            DataProvider.Instance.db.HD_ve_thang.Add(newAct);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateMonthAct(HD_ve_thang selected, string IDstop1, string IDstop2, DateTime? leave)
        {
            if (leave != null && selected.Gio_len >= leave.Value.TimeOfDay)
            {
                MessageBox.Show("Thời gian khách lên xuống không hợp lý");
                return;
            }

            var upt = DataProvider.Instance.db.HD_ve_thang.Where(x => x.Ma_ve == selected.Ma_ve
                                                                     && x.Ngay_su_dung == selected.Ngay_su_dung && x.Gio_len == selected.Gio_len).SingleOrDefault();
            upt.Ga_tram_len = IDstop1;
            upt.Ga_tram_xuong = IDstop2;
            if (leave != null) upt.Gio_xuong = leave.Value.TimeOfDay;

            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteMonthAct(HD_ve_thang selected)
        {
            DataProvider.Instance.db.HD_ve_thang.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
