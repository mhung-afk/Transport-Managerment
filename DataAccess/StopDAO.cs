using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class StopDAO
    {
        private static StopDAO instance;

        private StopDAO() { }

        public static StopDAO Instance
        {
            get { if (instance == null) instance = new StopDAO(); return instance; }
            private set => instance = value;
        }

        public Ga_Tram GetStop(string IDstop)
        {
            return DataProvider.Instance.db.Ga_Tram.Where(x => x.Ma_ga_tram == IDstop).SingleOrDefault();
        }

        public List<Ga_Tram> GetListStop()
        {
            return DataProvider.Instance.db.Ga_Tram.ToList();
        }

        public void AddNewStop(string IDstop, string name, string addr, byte type, string cross1, string cross2)
        {
            int testID;
            if (IDstop.Length != 7 || !(IDstop.StartsWith("BT") || IDstop.StartsWith("TT")) || !int.TryParse(IDstop.Substring(2), out testID))
                return;

            if((IDstop.StartsWith("BT") && type == 1) || (IDstop.StartsWith("TT") && type == 0))
            {
                MessageBox.Show("Mã ga/trạm và loại ga/trạm không tương ứng");
                return;
            }

            if (cross1 == cross2)
            {
                MessageBox.Show("2 giao lộ phải khác nhau");
                return;
            }

            int count = DataProvider.Instance.db.Ga_Tram.Where(x => x.Ma_ga_tram == IDstop).Count();
            if (count > 0)
            {
                MessageBox.Show("Mã ga/trạm đã tồn tại");
                return;
            }

            int isExist = DataProvider.Instance.db.Doan_duong.Where(x => x.Ma_giao_lo_1 == cross1 && x.Ma_giao_lo_2 == cross2).Count();
            if (isExist == 0)
            {
                MessageBox.Show("Đoạn đường giữa 2 giao lộ không có sẵn");
                return;
            }

            var newStop = new Ga_Tram()
            {
                Ma_ga_tram = IDstop,
                ten = name,
                dia_chi = addr,
                Ga_Tram1 = type,
                ma_giao_lo_1 = cross1,
                ma_giao_lo_2 = cross2
            };
            var temp = DataProvider.Instance.db.Ga_Tram.Add(newStop);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateStop(Ga_Tram selected, string name, string addr, string cross1, string cross2)
        {
            if (cross1 == cross2)
            {
                MessageBox.Show("2 giao lộ phải khác nhau");
                return;
            }

            int isExist = DataProvider.Instance.db.Doan_duong.Where(x => x.Ma_giao_lo_1 == cross1 && x.Ma_giao_lo_2 == cross2).Count();
            if (isExist == 0)
            {
                MessageBox.Show("Đoạn đường giữa 2 giao lộ không có sẵn");
                return;
            }

            var upt = DataProvider.Instance.db.Ga_Tram.Where(x => x.Ma_ga_tram == selected.Ma_ga_tram).SingleOrDefault();
            upt.ten = name;
            upt.dia_chi = addr;
            upt.ma_giao_lo_1 = cross1;
            upt.ma_giao_lo_2 = cross2;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteStop(Ga_Tram selected)
        {
            var acc = DataProvider.Instance.db.Ga_Tram.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
