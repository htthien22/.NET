using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDetailsDao
    {
        AceSportDB db = null;
        public OrderDetailsDao()
        {
            db = new AceSportDB();
        }

        public bool Insert(Tbl_OrderDetail detail)
        {
            try
            {
                db.Tbl_OrderDetail.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
