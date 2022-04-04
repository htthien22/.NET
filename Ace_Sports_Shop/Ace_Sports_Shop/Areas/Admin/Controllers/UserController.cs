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
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var dao = new UserDao();
            var model = dao.getALL(page, pagesize);
            return View(model);
        }

        //create user
        public JsonResult Register(string name, string pw, string rpw)
        {
            bool stt = false;
            var s = "";
            var dao = new UserDao();
            if (pw != rpw)
            {
                s = " Hai mật khẩu không giống nhau!";
                stt = false;
            }
            else if (dao.checkName(name))
            {
                s = " Tên đăng nhập đã tồn tại!";
                stt = false;
            }
            else
            {
                var user = new Tbl_User();
                user.UserName = name;
                user.Password = PWMD5.MD5Hash(pw);
                user.CreatedDate = DateTime.Now;
                user.Status = true;
                var x = dao.Insert(user);
                if (x > 0)
                {
                    stt = true;
                    s = "Đăng ký thành công!";
                }
                else
                {
                    stt = false;
                    s = "Đăng ký không thành công!";
                }
            }
            return Json(new { status = stt, noti = s }, JsonRequestBehavior.AllowGet);
        }

        //delte 
        public JsonResult Delete(long ID)
        {
            var s = "";
            new UserDao().delete(ID);
            return Json(new { status = true, noti = s }, JsonRequestBehavior.AllowGet);
        }


        //edit user get
        public JsonResult GetUser(long ID)
        {
            bool stt = true;
            var dao = new UserDao().getUser(ID);
            string id = dao.ID.ToString();
            string user = dao.UserName.ToString();
            string name = dao.Name.ToString();
            string pw = PWMD5.MD5De(dao.Password.ToString());
            string add = dao.Address.ToString();
            string phone = dao.Phone.ToString();

            return Json(new { status = stt, ID = id, User = user, Pw = pw, Name = name, Phone = phone, Add = add }, JsonRequestBehavior.AllowGet);
        }
        //edit user
        public JsonResult editOrder(long ID, string Name, string Phone, string Add)
        {
            bool stt = false;
            var check = new OrderDao().EditOrder(ID, Name, Phone, Add);
            if (check == true)
            {
                stt = true;
            }
            return Json(new { status = stt }, JsonRequestBehavior.AllowGet);
        }

        //edit user
        public JsonResult editUser(string User, string Name,string Pw, string Phone, string Add, string Status)
        {
            bool st = false;
            if (Status == "True")
            {
                st = true;
            }
            bool stt = false;
            var check = new UserDao().editUse(User, Name,PWMD5.MD5Hash(Pw), Phone, Add,st);
            if (check == true)
            {
                stt = true;
            }
            return Json(new { status = stt }, JsonRequestBehavior.AllowGet);
        }
    }
}