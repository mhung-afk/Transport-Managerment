using System.Collections.Generic;
using System.Linq;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    public class CrossDAO
    {
        private static CrossDAO instance;

        private CrossDAO() { }

        public static CrossDAO Instance
        {
            get { if (instance == null) instance = new CrossDAO(); return instance; }
            private set => instance = value;
        }

        private static int ID = 0;

        public List<Giao_lo> GetListCross()
        {
            return DataProvider.Instance.db.Giao_lo.ToList();
        }

        public Giao_lo GetCross(string IDcross)
        {
            return DataProvider.Instance.db.Giao_lo.Where(x => x.Ma_giao_lo == IDcross).SingleOrDefault();
        }

        public void AddNewCross(double? @long, double? lat)
        {
            var newCross = new Giao_lo()
            {
                Ma_giao_lo = ID.ToString(),
                @long = @long,
                lat = lat
            };
            ID++;
            var temp = DataProvider.Instance.db.Giao_lo.Add(newCross);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateCross(Giao_lo selected, double? @long, double? lat)
        {
            var acc = DataProvider.Instance.db.Giao_lo.Where(x => x.Ma_giao_lo == selected.Ma_giao_lo).SingleOrDefault();
            acc.@long = @long;
            acc.lat = lat;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteCross(Giao_lo selected)
        {
            var acc = DataProvider.Instance.db.Giao_lo.Remove(selected);
            DataProvider.Instance.db.SaveChanges();
        }
    }
}
