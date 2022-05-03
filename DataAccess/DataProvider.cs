using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class DataProvider
    {
        public Account curUser { get; set; }
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set => instance = value;
        }

        public QLPTCCEntities3 db { get; set; }

        private DataProvider()
        {
            db = new QLPTCCEntities3();
        }

        public bool CheckProxy()
        {
            return curUser.proxy == 0;
        }

        public bool CheckStaff()
        {
            return curUser.proxy != 2;
        }

        public void ResetConnection()
        {
            db = new QLPTCCEntities3();
        }

        public List<LoTrinhTuyenXeTau_Result> GetRouteByIdRoute(string IDroute)
        {
            return db.LoTrinhTuyenXeTau(IDroute).ToList();
        }

        public List<Thong_ke_so_luot_nguoi_Result> StatisticTurnByDay(string IDroute, DateTime? fromDate, DateTime? toDate)
        {
            var temp = db.Tuyen_tau_xe.Where(x => x.Ma_tuyen == IDroute).SingleOrDefault();
            if (temp == null)
            {
                MessageBox.Show("Mã tuyến không tồn tại");
                return null;
            }
            var temp2 = db.Thong_ke_so_luot_nguoi(IDroute, fromDate, toDate).ToList();
            return temp2;
        }
    }
}
