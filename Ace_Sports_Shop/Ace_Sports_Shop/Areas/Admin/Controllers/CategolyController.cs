using Ace_Sports_Shop.Common;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Areas.Admin.Controllers
{
    public class CategolyController : Controller
    {
        // GET: Admin/Categoly
        public ActionResult Index()
        {
            var dao = new ProductDAO().ListAll();
            return View(dao);
        }

        //create
        public JsonResult Create(string Name)
        {
            bool stt = false;
            var s = "";
            var cete = new CateDao();
            if (cete.checkName(Name))
            {
                s = "Tên danh mục đã tồn tại!";
            }else if (string.IsNullOrEmpty(Name))
            {
                s = "Tên danh mục không được bỏ trống!";
            }
            else
            {
                string meta = ConvertMeta.ToUnsignString(Name);
                cete.Create(Name, meta);
                stt = true;
                s = "Thêm danh mục thành công!";
            }
            return Json(new { status = stt,noti=s, JsonRequestBehavior.AllowGet});

        }

        //edit
        public JsonResult Edit(long ID)
        {
            bool stt = false;
            var s = "";
            var cete = new CateDao();
            
            return Json(new { status = stt, noti = s, JsonRequestBehavior.AllowGet });

        }
    }
}