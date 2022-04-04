using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CateDao
    {
        AceSportDB db = null;
        public CateDao()
        {
            db = new AceSportDB();
        }
        public bool checkName(string name)
        {
            return db.Tbl_ProductCategory.Count(x => x.Name == name) > 0;
        }
        public bool Create(string name, string meta)
        {
            try
            {
                var ca = new Tbl_ProductCategory();
                ca.Name = name;
                ca.MetaTitle = meta;
                ca.CreatedDate = DateTime.Now;
                db.Tbl_ProductCategory.Add(ca);
                db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
