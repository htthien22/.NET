using Ace_Sports_Shop.Common;
using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var dao = new ProductDAO().getAllProduct(page, pagesize);
            return View(dao);
        }
        public ActionResult CreateProduct()
        {
            var dao = new ProductDAO();
            ViewBag.CategoryID = dao.ListAll();
            return View();
        }
        //create 
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateProduct(Tbl_Product tb)
        {
            var dao = new ProductDAO();
            tb.MetaTitle = ConvertMeta.ToUnsignString(tb.Name);
            if (dao.InsertP(tb))
            {
                RedirectToAction("Index", "Product");
            }
            ViewBag.CategoryID = dao.ListAll();
            return View();
        }

        //edit
        [HttpGet]
        public ActionResult editProduct(long ID)
        {
            var dao = new ProductDAO().Details(ID);
            ViewBag.CategoryID = new ProductDAO().ListAll();
            return View(dao);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult editProduct(Tbl_Product tb)
        {
            if (ModelState.IsValid)
            {
                bool x = new ProductDAO().UpdateP(tb);
                if (x == true)
                {
                    SetAlert("Sửa thông tin sản phẩm thành công!", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAlert("Sửa thất bại!", "error");
                    return View();
                }
            }
            else
            {
                SetAlert("Các trường không được bỏ trống!", "error");
                var dao = new ProductDAO().Details(tb.ID);
                ViewBag.CategoryID = new ProductDAO().ListAll();
                return View(dao);
            }
                     
        }


        //delete
        public JsonResult Delete(long ID)
        {
            bool stt = true;
            new ProductDAO().delete(ID);
            return Json(new { status = stt }, JsonRequestBehavior.AllowGet);
        }

        //product out stock
        public ActionResult productout(int page = 1, int pagesize = 10)
        {
            var dao = new ProductDAO().pOut(page, pagesize);
            return View("Index", dao);
        }
    }
}