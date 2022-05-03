using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class TripDAO
    {
        private static TripDAO instance;

        private TripDAO() { }

        public static TripDAO Instance
        {
            get { if (instance == null) instance = new TripDAO(); return instance; }
            private set => instance = value;
        }

        public List<Chuyen_tau_xe_ghe_ga_tram> GetListTrip()
        {
            return DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.ToList();
        }

        public void AddNewTrip(string IDroute, byte triporder, string IDstop, byte stoporder, DateTime? come, DateTime? leave)
        {
            var temp = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == IDroute && x.STT_chuyen == triporder && x.Ma_ga_tram == IDstop).Count();
            if (temp > 0)
            {
                MessageBox.Show("Trùng lặp chuyến tàu/xe ghé ga/trạm");
                return;
            }

            if (come != null && leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian tàu/xe đến và rời không hợp lý");
                return;
            }

            var temp1 = DataProvider.Instance.db.Chuyen_tau_xe.Where(x => x.Ma_tuyen == IDroute && x.STT == triporder).Count();
            if (temp1 == 0)
            {
                MessageBoxResult res = MessageBox.Show("Chưa tồn tại chuyến tàu xe. Đồng ý mở rộng lộ trình?", "", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    DataProvider.Instance.db.Chuyen_tau_xe.Add(new Chuyen_tau_xe() { Ma_tuyen = IDroute, STT = triporder });
                    DataProvider.Instance.db.SaveChanges();
                }
                else return;
            }

            var newTrip = new Chuyen_tau_xe_ghe_ga_tram()
            {
                Ma_tuyen = IDroute,
                STT_chuyen = triporder,
                Ma_ga_tram = IDstop,
                STT_tram = stoporder
            };
            if (come != null) newTrip.gio_ghe = come.Value.TimeOfDay;
            if (leave != null) newTrip.gio_di = leave.Value.TimeOfDay;
            DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Add(newTrip);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateTrip(Chuyen_tau_xe_ghe_ga_tram selected, byte stoporder, DateTime? come, DateTime? leave)
        {
            if (come != null && leave != null && come >= leave)
            {
                MessageBox.Show("Thời gian tàu/xe đến và rời không hợp lý");
                return;
            }

            string id = selected.Ma_tuyen;
            byte stt = selected.STT_chuyen;

            var upt = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == selected.Ma_tuyen
                                                                                && x.STT_chuyen == selected.STT_chuyen
                                                                                && x.Ma_ga_tram == selected.Ma_ga_tram).SingleOrDefault();
            upt.STT_tram = stoporder;
            if (come != null) upt.gio_ghe = come.Value.TimeOfDay;
            if (leave != null) upt.gio_di = leave.Value.TimeOfDay;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteTrip(Chuyen_tau_xe_ghe_ga_tram selected)
        {
            string id = selected.Ma_tuyen;
            byte stt = selected.STT_chuyen;

            DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Remove(selected);
            DataProvider.Instance.db.SaveChanges();

            var temp2 = DataProvider.Instance.db.Chuyen_tau_xe_ghe_ga_tram.Where(x => x.Ma_tuyen == id && x.STT_chuyen == stt).Count();
            if (temp2 == 0)
            {
                var t = DataProvider.Instance.db;
                t.Chuyen_tau_xe.Remove(t.Chuyen_tau_xe.Where(x => x.Ma_tuyen == id && x.STT == stt).SingleOrDefault());
                t.SaveChanges();
            }
        }
    }
}
