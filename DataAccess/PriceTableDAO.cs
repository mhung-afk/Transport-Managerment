using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TransportManagerment.DataAccess
{
    public class PriceTableDAO
    {
        private static PriceTableDAO instance;

        private PriceTableDAO() { }

        public static PriceTableDAO Instance
        {
            get { if (instance == null) instance = new PriceTableDAO(); return instance; }
            private set => instance = value;
        }

        public void UpdatePrice(int p1, int p2, int p3)
        {
            try
            {
                var price = DataProvider.Instance.db.Bang_Gia.Where(x => x.STT == 1).SingleOrDefault();
                price.don_gia_xe_bus = p1;
                price.gia_ve_1_ngay_trong_tuan = p2;
                price.gia_ve_1_ngay_cuoi_tuan = p3;
                DataProvider.Instance.db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi");
                return;
            }
        }

        public List<int?> GetPrice()
        {
            var price = DataProvider.Instance.db.Bang_Gia.Where(x => x.STT == 1).SingleOrDefault();
            List<int?> res = new List<int?>();
            res.Add(price.don_gia_xe_bus);
            res.Add(price.gia_ve_1_ngay_trong_tuan);
            res.Add(price.gia_ve_1_ngay_cuoi_tuan);
            return res;
        }
    }
}
