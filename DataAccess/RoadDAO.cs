using System.Collections.Generic;
using System.Linq;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class RoadDAO
    {
        private static RoadDAO instance;

        private RoadDAO() { }

        public static RoadDAO Instance
        {
            get { if (instance == null) instance = new RoadDAO(); return instance; }
            private set => instance = value;
        }

        private static int ID = 0;

        public Con_duong GetRoad(string IDroad)
        {
            return DataProvider.Instance.db.Con_duong.Where(x => x.Ma_con_duong == IDroad).SingleOrDefault();
        }

        public List<Con_duong> GetListRoad()
        {
            return DataProvider.Instance.db.Con_duong.ToList();
        }

        public void AddNewRoad(string roadname)
        {
            var newRoad = new Con_duong()
            {
                Ma_con_duong = ID.ToString(),
                Ten_duong = roadname
            };
            ID++;
            var temp = DataProvider.Instance.db.Con_duong.Add(newRoad);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateRoad(Con_duong selected, string roadname)
        {
            var acc = DataProvider.Instance.db.Con_duong.Where(x => x.Ma_con_duong == selected.Ma_con_duong).SingleOrDefault();
            acc.Ten_duong = roadname;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteRoad(Con_duong selected)
        {
            var acc = DataProvider.Instance.db.Con_duong.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
