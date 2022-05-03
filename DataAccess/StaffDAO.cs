using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class StaffDAO
    {
        private static StaffDAO instance;

        private StaffDAO() { }

        public static StaffDAO Instance
        {
            get { if (instance == null) instance = new StaffDAO(); return instance; }
            private set => instance = value;
        }

        public List<Nhan_Vien> GetListStaff()
        {
            return DataProvider.Instance.db.Nhan_Vien.ToList();
        }

        public Nhan_Vien GetStaff(string ID)
        {
            return DataProvider.Instance.db.Nhan_Vien.Where(x => x.Ma_nhan_vien == ID).SingleOrDefault();
        }

        public void AddNewStaff(string IDStaff, string name, string typejob, DateTime? birthdate, string email, string sex, string telephone, string localphone)
        {
            int testID;
            if (IDStaff.Length != 6 || !IDStaff.StartsWith("NV") || !int.TryParse(IDStaff.Substring(2), out testID))
                return;

            if (sex == "Nam") sex = "M"; else sex = "F";

            int dupli1 = DataProvider.Instance.db.Nhan_Vien.Where(x => x.Ma_nhan_vien == IDStaff).Count();
            int dupli2 = DataProvider.Instance.db.Nhan_Vien.Where(x => x.dien_thoai_di_dong == telephone).Count();
            int dupli3 = DataProvider.Instance.db.Nhan_Vien.Where(x => x.dien_thoai_noi_bo == localphone).Count();
            if (dupli1 > 0 || dupli2 > 0 || dupli3 > 0)
            {
                MessageBox.Show("Trùng lặp mã nhân viên, hoặc SĐT nội bộ, hoặc SĐT di động");
                return;
            }

            var newStaff = new Nhan_Vien()
            {
                Ma_nhan_vien = IDStaff,
                ten = name,
                loai_cong_viec = typejob,
                ngay_sinh = birthdate,
                email = email,
                gioi_tinh = sex,
                dien_thoai_di_dong = telephone,
                dien_thoai_noi_bo = localphone
            };
            DataProvider.Instance.db.Nhan_Vien.Add(newStaff);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateStaff(Nhan_Vien selected, string name, string typejob, DateTime? birthdate, string email, string sex, string telephone, string localphone)
        {
            int dupli1 = DataProvider.Instance.db.Nhan_Vien.Where(x => x.dien_thoai_di_dong == telephone).Count();
            int dupli2 = DataProvider.Instance.db.Nhan_Vien.Where(x => x.dien_thoai_noi_bo == localphone).Count();
            if (selected.dien_thoai_di_dong != telephone && dupli1 > 0)
            {
                MessageBox.Show("Trùng lặp SĐT di động");
                return;
            }
            if (selected.dien_thoai_noi_bo != localphone && dupli2 > 0)
            {
                MessageBox.Show("Trùng lặp SĐT nội bộ");
                return;
            }

            if (sex == "Nam") sex = "M"; else sex = "F";

            var upt = DataProvider.Instance.db.Nhan_Vien.Where(x => x.Ma_nhan_vien == selected.Ma_nhan_vien).SingleOrDefault();
            upt.ten = name;
            upt.loai_cong_viec = typejob;
            upt.ngay_sinh = birthdate;
            upt.email = email;
            upt.gioi_tinh = sex;
            upt.dien_thoai_di_dong = telephone;
            upt.dien_thoai_noi_bo = localphone;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteStaff(Nhan_Vien selected)
        {
            var acc = DataProvider.Instance.db.Nhan_Vien.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
