using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class TicketDAO
    {
        private static TicketDAO instance;

        private TicketDAO() { }

        public static TicketDAO Instance
        {
            get { if (instance == null) instance = new TicketDAO(); return instance; }
            private set => instance = value;
        }

        public List<Ve> GetListTicket()
        {
            return DataProvider.Instance.db.Ves.ToList();
        }

        //public List<Ve> GetListOdd()
        //{
        //    return DataProvider.Instance.db.Ves.Where(x => x.Loai_ve == 0).ToList();
        //}

        public List<Ve> GetListDay()
        {
            return DataProvider.Instance.db.Ves.Where(x => x.Loai_ve == 2).ToList();
        }

        public List<Ve> GetListMonth()
        {
            return DataProvider.Instance.db.Ves.Where(x => x.Loai_ve == 1).ToList();
        }

        public Ve GetTicket(string ID)
        {
            return DataProvider.Instance.db.Ves.Where(x => x.Ma_ve == ID).SingleOrDefault();
        }

        public void AddNewTicket(string IDTicket, string type, DateTime? date, string IDcus)
        {
            int testID;
            if (IDTicket.Length != 5 || !int.TryParse(IDTicket, out testID))
                return;

            if (!string.IsNullOrEmpty(IDcus))
            {
                var temp1 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == IDcus).Count();
                if (temp1 == 0)
                {
                    MessageBox.Show("Không tồn tại hành khách mã: " + IDcus);
                    return;
                }
            }
            else IDcus = null;

            byte t;
            if (type == "Vé lẻ") t = 0;
            else if (type == "Vé tháng") t = 1;
            else t = 2;

            if (date == null)
            {
                date = DateTime.Now;
            }
            var temp = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve.Substring(10) == IDTicket && x.Loai_ve == t
                                                                && DbFunctions.TruncateTime(x.Ngay_gio_mua) == DbFunctions.TruncateTime(date)).Count();
            if (temp > 0)
            {
                MessageBox.Show("Trùng lặp mã vé.");
                return;
            }

            var newTicket = new Ve()
            {
                Ma_ve = IDTicket,
                Loai_ve = t,
                Ngay_gio_mua = date,
                Ma_khach_hang = IDcus
            };
            DataProvider.Instance.db.Ves.Add(newTicket);
            DataProvider.Instance.db.SaveChanges();

            var e = DataProvider.Instance.db.Ves.Where(x => x.Loai_ve == t
                                                    && DbFunctions.TruncateTime(x.Ngay_gio_mua) == DbFunctions.TruncateTime(date)
                                                    && x.Ma_ve.Substring(10) == IDTicket).SingleOrDefault();

            if (t == 0)
            {
                DataProvider.Instance.db.Ve_le.Add(new Ve_le() { Ma_ve = e.Ma_ve });
            }
            else if (t == 1)
            {
                DataProvider.Instance.db.Ve_thang.Add(new Ve_thang() { Ma_ve = e.Ma_ve });
            }
            else
            {
                DataProvider.Instance.db.Ve_1_ngay.Add(new Ve_1_ngay() { Ma_ve = e.Ma_ve });
            }
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateTicket(Ve selected, string type, DateTime? date, string IDcus)
        {
            if (!string.IsNullOrEmpty(IDcus))
            {
                var temp1 = DataProvider.Instance.db.Hanh_Khach.Where(x => x.Ma_hanh_khach == IDcus).Count();
                if (temp1 == 0)
                {
                    MessageBox.Show("Không tồn tại hành khách mã: " + IDcus);
                    return;
                }
            }
            else IDcus = null;

            byte t;
            if (type == "Vé lẻ") t = 0;
            else if (type == "Vé tháng") t = 1;
            else t = 2;

            if (date == null)
            {
                date = DateTime.Now;
            }
            var temp = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve.Substring(10) == selected.Ma_ve.Substring(10) && x.Loai_ve == t
                                                                && DbFunctions.TruncateTime(x.Ngay_gio_mua) == DbFunctions.TruncateTime(date)).Count();
            if (temp > 1)
            {
                MessageBox.Show("Trùng lặp mã vé.");
                return;
            }

            var upt = DataProvider.Instance.db.Ves.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
            upt.Loai_ve = t;
            upt.Ngay_gio_mua = date;
            upt.Ma_khach_hang = IDcus;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteTicket(Ve selected)
        {
            var db = DataProvider.Instance.db;
            if (selected.Loai_ve == 0)
            {
                var t = db.Ve_le.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
                db.Ve_le.Remove(t);
            }
            else if (selected.Loai_ve == 1)
            {
                var d = db.HD_ve_thang.Where(x => x.Ma_ve == selected.Ma_ve).ToList();
                foreach (HD_ve_thang item in d)
                {
                    db.HD_ve_thang.Remove(item);
                }

                var t = db.Ve_thang.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
                db.Ve_thang.Remove(t);
            }
            else
            {
                var d = db.HD_ve_ngay.Where(x => x.Ma_ve == selected.Ma_ve).ToList();
                foreach (HD_ve_ngay item in d)
                {
                    db.HD_ve_ngay.Remove(item);
                }

                var t = db.Ve_1_ngay.Where(x => x.Ma_ve == selected.Ma_ve).SingleOrDefault();
                db.Ve_1_ngay.Remove(t);
            }

            DataProvider.Instance.db.Ves.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
