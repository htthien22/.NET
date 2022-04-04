using Model.Entity;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductDAO
    {
        AceSportDB db = null;
        public ProductDAO()
        {
            db = new AceSportDB();
        }
        /// <summary>
        /// get flash sale
        /// </summary>
        /// <returns></returns>
        public List<Tbl_Product> FlashSale()
        {
            return db.Tbl_Product.Where(x => x.Status == true && x.FlashSale == true).ToList();
        }
        //fomat number 
        public string formatNumber(int strNum)
        {

            string newNum = string.Format("{0:0,0}", strNum);
            return newNum + "₫";
        }

        public int sale(int oldprice, int newprice)
        {
            int pt = oldprice - newprice;
            float x = (float)pt / (float)oldprice;
            float re = x * 100;
            int xx = (int)re;
            return xx;
        }
        public List<Tbl_Product> FootBall(int top)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == 1).OrderBy(y => y.CreatedDate).Take(top).ToList();
        }
        public List<Tbl_Product> yoga(int top)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == 2).OrderBy(y => y.CreatedDate).Take(top).ToList();
        }
        public List<Tbl_Product> gym(int top)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == 3).OrderBy(y => y.CreatedDate).Take(top).ToList();
        }

        public List<Tbl_Product> pk(int top)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == 4).OrderBy(y => y.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// get list best seller
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Tbl_Product> bestseller(int top)
        {
            return db.Tbl_Product.Where(x => x.Status == true).OrderBy(y => y.CreatedDate).Take(top).ToList();
        }

        //product sugg
        public List<Tbl_Product> sugg(long id)
        {
            var product = db.Tbl_Product.Find(id);
            return db.Tbl_Product.Where(x => x.CategoryID == product.CategoryID && x.ID != id).OrderBy(y => y.CreatedDate).Take(10).ToList();
        }
        /// <summary>
        /// get list product by ID
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Tbl_Product> getFB(int page, int pagesize, int id)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == id).OrderBy(x => x.CreatedDate).ToPagedList(page, pagesize);
        }

        //get name product
        public List<Tbl_ProductCategory> getName(long id)
        {
            return db.Tbl_ProductCategory.Where(x => x.ID == id).ToList();
        }

        //product details
        public Tbl_Product Details(long id)
        {
            return db.Tbl_Product.Find(id);
        }


        //list name search
        public List<string> ListName(string keyword)
        {
            return db.Tbl_Product.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        //search
        public IEnumerable<ProductViewModel> Search(string keyword, ref int totalRecord, int page, int pagesize)
        {
            totalRecord = db.Tbl_Product.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Tbl_Product
                         join b in db.Tbl_ProductCategory
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.PromotionPrice
                         });
            model.OrderByDescending(x => x.CreatedDate);
            return model.ToPagedList(page, pagesize);
        }

        //get all product admin
        public IEnumerable<ProductViewModel> getAllProduct(int page, int pagesize)
        {
            var model = (from a in db.Tbl_Product
                         join b in db.Tbl_ProductCategory
                         on a.CategoryID equals b.ID
                         where a.CategoryID == b.ID
                         select new
                         {
                             msp = a.ID,
                             name = a.Name,
                             code = a.Code,
                             image = a.Image,
                             create = a.CreatedDate,
                             price = a.Price,
                             pricem = a.PromotionPrice,
                             qty = a.Quantity,
                             wty = a.Warranty,
                             cater = b.Name,
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             ID = x.msp,
                             Name = x.name,
                             Images = x.image,
                             Code = x.code,
                             CreatedDate = x.create,
                             Price = x.price,
                             PromotionPrice = x.pricem,
                             Quantity = x.qty,
                             Warranty = x.wty,
                             CateName = x.cater,
                         });
            model.OrderBy(x => x.CreatedDate);
            return model.ToPagedList(page, pagesize);
        }


        public List<Tbl_ProductCategory> ListAll()
        {
            return db.Tbl_ProductCategory.Where(x => x.Status == true).ToList();
        }

        //create product
        public bool InsertP(Tbl_Product p)
        {
            p.Status = true;
            p.Warranty = p.Quantity;
            p.CreatedDate = DateTime.Now;
            db.Tbl_Product.Add(p);
            db.SaveChanges();
            return true;
        }

        //edit product
        public bool UpdateP(Tbl_Product tb)
        {
            var c = db.Tbl_Product.SingleOrDefault(x => x.ID == tb.ID);
            if(c != null)
            {
                c.Name = tb.Name;
                c.Price = tb.Price;
                c.PromotionPrice = tb.PromotionPrice;
                c.Quantity = tb.Quantity;
                c.Image = tb.Image;
                c.CategoryID = tb.CategoryID;
                c.Description = tb.Description;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //get prododuct edit
        //public Tbl_Product  getEdtP()
        //{
        //    return  db.Tbl_Product.FirstOrDefault(y=>y.ID == id);
        //}
        //public List<Tbl_ProductCategory> getEdtP(long id)
        //{
        //    return db.Tbl_ProductCategory.Where(x => x.ID == id).ToList();
        //}
        //delete product
        public bool delete(long id)
        {

            try
            {
                var del = db.Tbl_Product.SingleOrDefault(x => x.ID == id);
                db.Tbl_Product.Remove(del);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //update qty 
        public bool qty(long id, int qt)
        {
            var x = db.Tbl_Product.SingleOrDefault(v => v.ID == id);
            x.Warranty = x.Warranty - qt;
            db.SaveChanges();
            return true;
        }

        //rate product
        public bool Rate(long pid, long uid, string content)
        {
            var f = new Tbl_Feedback();
            f.userID = uid;
            f.productID = pid;
            f.Content = content;
            f.CreatedDate = DateTime.Now;
            f.Status = true;
            db.Tbl_Feedback.Add(f);
            db.SaveChanges();
            return true;
        }

        //check uid rate product
        public Tbl_Feedback CheckRate(long pid, long uid)
        {
            var c = db.Tbl_Feedback.Where(x => x.userID == uid && x.productID == pid).FirstOrDefault();
            return c;
        }

        //get list rate product 

        public List<Tbl_Feedback> getListRate(long ID)
        {
            var rate = db.Tbl_Feedback.Where(x => x.productID == ID).OrderBy(c=>c.CreatedDate);
            return rate.ToList();
        }
        public List<Tbl_Feedback> getListRateI()
        {
            var rate = db.Tbl_Feedback.OrderBy(c => c.CreatedDate);
            return rate.ToList();
        }

        //short by price down
        public IEnumerable<Tbl_Product> PriceDow(int page, int pagesize, int id)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == id).OrderByDescending(x => x.PromotionPrice).ToPagedList(page, pagesize);
        }

        //short by price up
        public IEnumerable<Tbl_Product> PriceUp(int page, int pagesize, int id)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == id).OrderBy(x => x.PromotionPrice).ToPagedList(page, pagesize);
        }

        //short by date
        public IEnumerable<Tbl_Product> ProductDate(int page, int pagesize, int id)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == id).OrderBy(x => x.CreatedDate).ToPagedList(page, pagesize);
        }

        //get all product
        public IEnumerable<Tbl_Product> GetAll(int page, int pagesize, int id)
        {
            return db.Tbl_Product.Where(x => x.CategoryID == id).ToPagedList(page, pagesize);
        }

        //product out of stiock
        public int ProductOut()
        {
            var x = db.Tbl_Product.Where(a => a.Warranty < 10).Count();
            return x;
        }

        public IEnumerable<Tbl_Product> pOut(int page, int pagesize)
        {
            return db.Tbl_Product.Where(x=>x.Warranty < 10).OrderBy(a=>a.ID).ToPagedList(page, pagesize);
        }

    }
}
