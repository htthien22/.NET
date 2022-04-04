using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;

namespace Ace_Sports_Shop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Total = new OrderDao().TotalOrder();
            ViewBag.TotalDay = new OrderDao().TotalOrderDay();
            ViewBag.ProductOut = new ProductDAO().ProductOut();
            ViewBag.TotalPrice = new OrderDao().TotalPrice();
            return View();
        }
    }
}