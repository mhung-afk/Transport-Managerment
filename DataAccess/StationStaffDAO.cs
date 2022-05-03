using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class StationStaffDAO
    {
        private static StationStaffDAO instance;

        private StationStaffDAO() { }

        public static StationStaffDAO Instance
        {
            get { if (instance == null) instance = new StationStaffDAO(); return instance; }
            private set => instance = value;
        }

        public List<Ga_Tram_Lam_Viec> GetListStationStaff()
        {
            return DataProvider.Instance.db.Ga_Tram_Lam_Viec.ToList();
        }

        public void AddNewStationStaff(string ID, string IDstop)
        {
            var temp = DataProvider.Instance.db.Ga_Tram_Lam_Viec.Where(x => x.Ma_nhan_vien == ID).Count();
            if (temp > 0)
            {
                MessageBox.Show("Mỗi nhân viên chỉ làm việc ở một ga hoặc trạm.");
                return;
            }

            var newStationStaff = new Ga_Tram_Lam_Viec()
            {
                Ma_nhan_vien = ID,
                Ma_ga_tram = IDstop
            };
            DataProvider.Instance.db.Ga_Tram_Lam_Viec.Add(newStationStaff);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateStationStaff(Ga_Tram_Lam_Viec selected, string IDstop)
        {
            var upt = DataProvider.Instance.db.Ga_Tram_Lam_Viec.Where(x => x.Ma_nhan_vien == selected.Ma_nhan_vien).SingleOrDefault();
            upt.Ma_ga_tram = IDstop;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteStationStaff(Ga_Tram_Lam_Viec selected)
        {
            DataProvider.Instance.db.Ga_Tram_Lam_Viec.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
