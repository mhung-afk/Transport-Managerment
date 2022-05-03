using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class MagCardDAO
    {
        private static MagCardDAO instance;

        private MagCardDAO() { }

        public static MagCardDAO Instance
        {
            get { if (instance == null) instance = new MagCardDAO(); return instance; }
            private set => instance = value;
        }

        public List<The_Tu> GetListMagCard()
        {
            return DataProvider.Instance.db.The_Tu.ToList();
        }

        public void AddNewMagCard(string ID, DateTime? date, string IDcus)
        {
            int testID;
            if (ID.Length != 8 || !ID.StartsWith("TT") || !int.TryParse(ID.Substring(2), out testID))
                return;

            var temp = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == IDcus).SingleOrDefault();
            if (temp == null)
            {
                MessageBox.Show("Mã hành khách không tồn tại");
                return;
            }

            int count = DataProvider.Instance.db.The_Tu.Where(x => x.Ma_the_tu == ID).Count();
            if (count > 0)
            {
                MessageBox.Show("Trùng lặp mã thẻ từ");
                return;
            }

            var temp1 = DataProvider.Instance.db.The_Tu.Where(x => x.Ma_hanh_khach == IDcus).SingleOrDefault();
            if (temp1 != null)
            {
                MessageBox.Show("Hành khách đã được cấp thẻ từ");
                return;
            }

            if (date == null) date = DateTime.Now;

            var newMagCard = new The_Tu()
            {
                Ma_the_tu = ID,
                Ngay_mua = date,
                Ma_hanh_khach = IDcus
            };

            DataProvider.Instance.db.The_Tu.Add(newMagCard);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateMagCard(The_Tu selected, DateTime? date, string IDcus)
        {
            var temp = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == IDcus).SingleOrDefault();
            if (temp == null)
            {
                MessageBox.Show("Mã hành khách không tồn tại");
                return;
            }

            var temp1 = DataProvider.Instance.db.The_Tu.Where(x => x.Ma_hanh_khach == IDcus && x.Ma_the_tu != selected.Ma_the_tu).SingleOrDefault();
            if (temp1 != null)
            {
                MessageBox.Show("Hành khách đã được cấp thẻ từ");
                return;
            }

            if (date == null) date = DateTime.Now;

            var upt = DataProvider.Instance.db.The_Tu.Where(x => x.Ma_the_tu == selected.Ma_the_tu).SingleOrDefault();
            upt.Ngay_mua = date;
            upt.Ma_hanh_khach = IDcus;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteMagCard(The_Tu selected)
        {
            DataProvider.Instance.db.The_Tu.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
