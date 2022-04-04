using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ace_Sports_Shop.Areas.Admin.Models;
using Ace_Sports_Shop.Common;
using Model.DAO;

namespace Ace_Sports_Shop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var ex = dao.Login(login.UserName, PWMD5.MD5Hash(login.PassWord));
                if (ex == 1)
                {
                    var user = dao.getName(login.UserName);
                    var userSession = new UserLogin();
                    userSession.Name = user.UserName;
                    userSession.ID = user.ID;
                    Session.Add(CommonStand.User_Session, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if(ex == -1)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu không chính xác!");
                }
                else if(ex == 0)
                {
                    ModelState.AddModelError("TenNguoiDung","Tên tài khoản không tồn tại!");
                }else if(ex == -2)
                {
                    ModelState.AddModelError("MatKhau", "Tài khoản bị khóa!");
                }
                            
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToRoute("logout");           
        }
    }
}