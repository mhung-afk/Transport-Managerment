using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class ConsumerDAO
    {
        private static ConsumerDAO instance;

        private ConsumerDAO() { }

        public static ConsumerDAO Instance
        {
            get { if (instance == null) instance = new ConsumerDAO(); return instance; }
            private set => instance = value;
        }

        public List<Hanh_Khach> GetListConsumer()
        {
            return DataProvider.Instance.db.Hanh_Khach.ToList();
        }

        public void AddNewConsumer(string IDconsumer, string name, string cmnd_cccd, string job, string phone, string sex, string email, DateTime? birthdate)
        {
            int testID;
            if (IDconsumer.Length != 8 || !IDconsumer.StartsWith("KH") || !int.TryParse(IDconsumer.Substring(2), out testID))
                return;

            if (sex == "Nam") sex = "M"; else sex = "F";

            int dupli1 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == IDconsumer).Count();
            int dupli2 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.CMND_CCCD == cmnd_cccd).Count();
            int dupli3 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.dien_thoai == phone).Count();
            if (dupli1 > 0 || dupli2 > 0 || dupli3 > 0)
            {
                MessageBox.Show("Trùng lặp mã hành khách, hoặc CMND/CCCD, hoặc SĐT");
                return;
            }

            var newConsumer = new Hanh_Khach()
            {
                Ma_hanh_khach = IDconsumer,
                ten = name,
                CMND_CCCD = cmnd_cccd,
                nghe_nghiep = job,
                dien_thoai = phone,
                gioi_tinh = sex,
                email = email,
                ngay_sinh = birthdate
            };
            DataProvider.Instance.db.Hanh_Khach.Add(newConsumer);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateConsumer(Hanh_Khach selected, string name, string cmnd_cccd, string job, string phone, string sex, string email, DateTime? birthdate)
        {
            int dupli1 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.CMND_CCCD == cmnd_cccd).Count();
            int dupli2 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.dien_thoai == phone).Count();
            if (selected.CMND_CCCD != cmnd_cccd && dupli1 > 0)
            {
                MessageBox.Show("Trùng lặp CMND/CCCD");
                return;
            }
            if (selected.dien_thoai != phone && dupli2 > 0)
            {
                MessageBox.Show("Trùng lặp SĐT");
                return;
            }

            if (sex == "Nam") sex = "M"; else sex = "F";
            var upt = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == selected.Ma_hanh_khach).SingleOrDefault();
            upt.ten = name;
            upt.CMND_CCCD = cmnd_cccd;
            upt.nghe_nghiep = job;
            upt.dien_thoai = phone;
            upt.gioi_tinh = sex;
            upt.email = email;
            upt.ngay_sinh = birthdate;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteConsumer(Hanh_Khach selected)
        {
            var t = DataProvider.Instance.db.The_Tu.Where(x => x.Ma_hanh_khach == selected.Ma_hanh_khach).SingleOrDefault();
            if (t != null)
            {
                DataProvider.Instance.db.The_Tu.Remove(t);
            }

            DataProvider.Instance.db.Hanh_Khach.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
