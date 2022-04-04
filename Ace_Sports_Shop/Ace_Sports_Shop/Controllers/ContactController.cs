using Ace_Sports_Shop.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Send(string name, string mobile, string email, string content)
        {
            string contentx = System.IO.File.ReadAllText(Server.MapPath("~/Content/Contact.html"));
            contentx = contentx.Replace("{{Name}}", name);
            contentx = contentx.Replace("{{Email}}", email);
            contentx = contentx.Replace("{{Mobile}}", mobile);
            contentx = contentx.Replace("{{Content}}", content);
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            try
            {                
                new SendMail().Send(toEmail, "YÊU CẦU MỚI TỪ KHÁCH HÀNG", contentx);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}