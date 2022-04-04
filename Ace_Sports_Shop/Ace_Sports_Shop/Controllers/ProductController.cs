using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Controllers
{
    public class ProductController : Controller
    {
        public static int cateid = 0;
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ProductFB()
        {
            var model = new ProductDAO().FootBall(12);
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult ProductGym()
        {
            var model = new ProductDAO().gym(12);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductYoga()
        {
            var model = new ProductDAO().yoga(12);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductPK()
        {
            var model = new ProductDAO().pk(12);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductBest()
        {
            var model = new ProductDAO().bestseller(10);
            return PartialView(model);
        }
        //product category
        public ActionResult ProductCategory(int page = 1, int pagesize = 8, int id = 0)
        {
            cateid = id;
            var dao = new ProductDAO().getFB(page, pagesize, id);
            ViewBag.name = new ProductDAO().getName(id);
            return View(dao);
        }

        //short by product
        public ActionResult ShortBy(string By)
        {
            ViewBag.by = "";
            //var dao = new ProductDAO();
            int page = 1, pagesize = 12, id = cateid;
            ViewBag.name = new ProductDAO().getName(id);
            var dao = (dynamic)null;
            if (By == "PriceDown")
            {
                dao = new ProductDAO().PriceDow(page, pagesize, id);
            }
            else if (By == "PriceUp")
            {
                dao = new ProductDAO().PriceUp(page, pagesize, id);
            }
            else if (By == "New")
            {
                dao = new ProductDAO().ProductDate(page, pagesize, id);
            }
            else
            {
                dao = new ProductDAO().GetAll(page, pagesize, id);
            }
            return View(dao);
        }

        //details product
        public ActionResult ProductDetails(long id)
        {
            var model = new ProductDAO().Details(id);
            ViewBag.name = new ProductDAO().getName(id);
            ViewBag.rate = new ProductDAO().getListRate(id);
            ViewBag.product = new ProductDAO().sugg(id);
            return View(model);
        }

        //list name search
        public JsonResult ListName(string q)
        {
            var data = new ProductDAO().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //search
        public ActionResult Search(string search, int page = 1, int pagesize = 8)
        {
            int totalRecord = 0;
            var model = new ProductDAO().Search(search, ref totalRecord, page, pagesize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = search;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pagesize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        //rate product
        public JsonResult RateProduct(long IDP, long IDU, string Content)
        {
            bool stt = false;
            string s = "Bạn đã nhận xét sản phẩm này rồi!";
            var c = new ProductDAO();
            if (c.CheckRate(IDP, IDU) == null)
            {
                c.Rate(IDP, IDU, Content);
                stt = true;
                s = "Nhận xét thành công!";
            }
            return Json(new { status = stt, noti = s }, JsonRequestBehavior.AllowGet);
        }

        //product quick view
        public JsonResult QuickViewProDuct(long ID)
        {
            var model = new ProductDAO().Details(ID);
            return Json(new { status = false, product = model }, JsonRequestBehavior.AllowGet);
        }
    }
}