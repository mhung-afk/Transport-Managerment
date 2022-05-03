using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class TrainDAO
    {
        private static TrainDAO instance;

        private TrainDAO() { }

        public static TrainDAO Instance
        {
            get { if (instance == null) instance = new TrainDAO(); return instance; }
            private set => instance = value;
        }

        public List<Tuyen_tau_dien> GetListTrain()
        {
            return DataProvider.Instance.db.Tuyen_tau_dien.ToList();
        }

        public void AddNewTrain(string code, string name, int price, string IDtrain)
        {
            int testID;
            if (IDtrain.Length != 4 || !IDtrain.StartsWith("T") || !int.TryParse(IDtrain.Substring(1), out testID))
                return;

            if (code.Length != 1 || code[0] < 'A' || code[0] > 'Z')
            {
                MessageBox.Show("Mã tàu điện phải là 1 chữ cái in hoa");
                return;
            }

            int count = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == IDtrain).Count();
            if (count == 0)
            {
                DataProvider.Instance.db.Tuyen_tau_xe.Add(new Tuyen_tau_xe() { Ma_tuyen = IDtrain });
                DataProvider.Instance.db.SaveChanges();
            }

            int count1 = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ma_tuyen_tau == code).Count();
            if (count1 > 0)
            {
                MessageBox.Show("Trùng lặp mã tuyến tàu");
                return;
            }

            int count2 = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ten_tuyen_tau == name).Count();
            if (count2 > 0)
            {
                MessageBox.Show("Trùng lặp tên tuyến tàu");
                return;
            }

            var newTrain = new Tuyen_tau_dien()
            {
                Ma_tuyen_tau = code[0].ToString(),
                Ten_tuyen_tau = name,
                Don_gia = price,
                Ma_tuyen_tau_dien = IDtrain
            };

            DataProvider.Instance.db.Tuyen_tau_dien.Add(newTrain);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateTrain(Tuyen_tau_dien selected, string name, int price, string IDtrain)
        {
            int testID;
            if (IDtrain.Length != 4 || !IDtrain.StartsWith("T") || !int.TryParse(IDtrain.Substring(1), out testID))
                return;

            int count3 = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ten_tuyen_tau == name).Count();
            if (count3 > 0 && selected.Ten_tuyen_tau != name)
            {
                MessageBox.Show("Trùng lặp tên tuyến tàu");
                return;
            }

            int count1 = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == IDtrain).Count();
            if (count1 == 0)
            {
                DataProvider.Instance.db.Tuyen_tau_xe.Add(new Tuyen_tau_xe() { Ma_tuyen = IDtrain });
                DataProvider.Instance.db.SaveChanges();
            }

            string id = selected.Ma_tuyen_tau_dien;

            var upt = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ma_tuyen_tau == selected.Ma_tuyen_tau).SingleOrDefault();
            upt.Ten_tuyen_tau = name;
            upt.Don_gia = price;
            upt.Ma_tuyen_tau_dien = IDtrain;
            DataProvider.Instance.db.SaveChanges();

            int count2 = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ma_tuyen_tau_dien == id).Count();
            if (count2 == 0)
            {
                Tuyen_tau_xe t = new Tuyen_tau_xe();
                t = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == id).SingleOrDefault();
                DataProvider.Instance.db.Tuyen_tau_xe.Remove(t);
                DataProvider.Instance.db.SaveChanges();
            }
        }

        public void DeleteTrain(Tuyen_tau_dien selected)
        {
            string id = selected.Ma_tuyen_tau_dien;
            DataProvider.Instance.db.Tuyen_tau_dien.Remove(selected);
            DataProvider.Instance.db.SaveChanges();

            int count2 = DataProvider.Instance.db.Tuyen_tau_dien.Where(x => x.Ma_tuyen_tau_dien == selected.Ma_tuyen_tau_dien).Count();
            if (count2 == 0)
            {
                Tuyen_tau_xe t = new Tuyen_tau_xe();
                t = DataProvider.Instance.db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == id).SingleOrDefault();
                DataProvider.Instance.db.Tuyen_tau_xe.Remove(t);
                DataProvider.Instance.db.SaveChanges();
            }
        }
    }
}
