using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class DayActDAO
    {
        private static DayActDAO instance;

        private DayActDAO() { }

        public static DayActDAO Instance
        {
            get { if (instance == null) instance = new DayActDAO(); return instance; }
            private set => instance = value;
        }

        public List<HD_ve_ngay> GetListDayAct()
        {
            return DataProvider.Instance.db.HD_ve_ngay.ToList();
        }

        private byte CountActByID(string ID)
        {
            var t = DataProvider.Instance.db.HD_ve_ngay.Where(x => x.Ma_ve == ID).OrderByDescending(x => x.STT).FirstOrDefault();
            if (t != null)
            {
                return (byte)t.STT;
            }
            else
            {
                return 0;
            }
        }

        public void AddDayAct(string ID, string IDroute, string IDstop1, string IDstop2, DateTime? come, DateTime? leave)
        {
            var temp = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop1).Count();
            if (temp == 0)
            {
                MessageBox.Show("Tuyến " + IDroute + " không đi qua ga/trạm " + IDstop1);
                return;
            }

            var temp1 = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop2).Count();
            if (temp1 == 0)
            {
                MessageBox.Show("Tuyến " + IDroute + " không đi qua ga/trạm " + IDstop2);
                return;
            }

            if (come != null && leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian khách lên xuống không hợp lý");
                return;
            }

            byte order = CountActByID(ID);
            order++;
            var newAct = new HD_ve_ngay()
            {
                Ma_ve = ID,
                STT = order,
                Ma_tuyen = IDroute,
                Ma_ga_tram_len = IDstop1,
                Ma_ga_tram_xuong = IDstop2,
            };

            if (come != null) newAct.Gio_len = come.Value.TimeOfDay;
            if (leave != null) newAct.Gio_xuong = leave.Value.TimeOfDay;

            DataProvider.Instance.db.HD_ve_ngay.Add(newAct);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateDayAct(HD_ve_ngay selected, string IDroute, string IDstop1, string IDstop2, DateTime? come, DateTime? leave)
        {
            var temp = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop1).Count();
            if (temp == 0)
            {
                MessageBox.Show("Tuyến " + IDroute + " không đi qua ga/trạm " + IDstop1);
                return;
            }

            var temp1 = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.Ma_ga_tram == IDstop2).Count();
            if (temp1 == 0)
            {
                MessageBox.Show("Tuyến " + IDroute + " không đi qua ga/trạm " + IDstop2);
                return;
            }

            if (come != null && leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian khách lên xuống không hợp lý");
                return;
            }

            var upt = DataProvider.Instance.db.HD_ve_ngay.Where(x => x.Ma_ve == selected.Ma_ve && x.STT == selected.STT).SingleOrDefault();
            upt.Ma_tuyen = IDroute;
            upt.Ma_ga_tram_len = IDstop1;
            upt.Ma_ga_tram_xuong = IDstop2;

            if (leave != null) upt.Gio_xuong = leave.Value.TimeOfDay;
            if (come != null) upt.Gio_len = come.Value.TimeOfDay;

            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteDayAct(HD_ve_ngay selected)
        {
            DataProvider.Instance.db.HD_ve_ngay.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
