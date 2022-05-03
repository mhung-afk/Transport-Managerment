using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class DistanceDAO
    {
        private static DistanceDAO instance;

        private DistanceDAO() { }

        public static DistanceDAO Instance
        {
            get { if (instance == null) instance = new DistanceDAO(); return instance; }
            private set => instance = value;
        }

        public List<Doan_duong> GetListDistance()
        {
            return DataProvider.Instance.db.Doan_duong.ToList();
        }

        public void AddNewDistance(string cross1, string cross2, string road, int STT)
        {
            //try
            //{
            //    if(cross1==cross2)
            //    {
            //        MessageBox.Show("2 giao lộ phải khác nhau");
            //        return;
            //    }
            //    var newDistance = new Doan_duong()
            //    {
            //        Ma_giao_lo_1 = cross1,
            //        Ma_giao_lo_2 = cross2,
            //        Ma_con_duong = road,
            //        STT = STT
            //    };
            //    var temp = DataProvider.Instance.db.Doan_duong.Add(newDistance);
            //    DataProvider.Instance.db.SaveChanges();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Trùng lặp đoạn đường");
            //    return;
            //}
            if (cross1 == cross2)
            {
                MessageBox.Show("2 giao lộ phải khác nhau");
                return;
            }

            int count = DataProvider.Instance.db.Doan_duong.Where(x => x.Ma_giao_lo_1 == cross1 && x.Ma_giao_lo_2 == cross2).Count();
            if (count > 0)
            {
                MessageBox.Show("Trùng lặp đoạn đường");
                return;
            }

            var newDistance = new Doan_duong()
            {
                Ma_giao_lo_1 = cross1,
                Ma_giao_lo_2 = cross2,
                Ma_con_duong = road,
                STT = STT
            };
            var temp = DataProvider.Instance.db.Doan_duong.Add(newDistance);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateDistance(Doan_duong selected, string road, int STT)
        {
            var upt = DataProvider.Instance.db.Doan_duong.Where(x => x.Ma_giao_lo_1 == selected.Ma_giao_lo_1
                                                            && x.Ma_giao_lo_2 == selected.Ma_giao_lo_2).SingleOrDefault();
            upt.Ma_con_duong = road;
            upt.STT = STT;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteDistance(Doan_duong selected)
        {
            try
            {
                var acc = DataProvider.Instance.db.Doan_duong.Remove(selected);
                DataProvider.Instance.db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xoá đoạn đường đang tồn tại ga/trạm");
                return;
            }

        }
    }
}
