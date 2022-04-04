using Ace_Sports_Shop.Common;
using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            var product = new ProductDAO();
            var model = product.FlashSale();
            ViewBag.rate = new ProductDAO().getListRateI();
            return View(model);
        }
        [ChildActionOnly]
        public PartialViewResult HeaderHome()
        {        

            return PartialView();
        }

        //mini cart
        [ChildActionOnly]
        public PartialViewResult MiniCart()
        {
            var cart = Session[CommonStand.Cart_Session];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}