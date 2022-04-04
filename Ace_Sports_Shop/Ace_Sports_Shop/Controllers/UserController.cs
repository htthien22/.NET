using Ace_Sports_Shop.Common;
using Ace_Sports_Shop.Models;
using ASPSnippets.GoogleAPI;
using BotDetect.Web.Mvc;
using Facebook;
using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Ace_Sports_Shop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.id;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new Tbl_User();
                user.Email = email;
                user.UserName = userName;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.Name = user.Name;
                    userSession.ID = user.ID;
                    Session.Add(CommonStand.User_Session, userSession);

                }
            }
            return Redirect("/");
        }
        //login
        [HttpPost]
        public JsonResult Login(string username, string pw)
        {
            bool stt = false;
            var s = "";
            var dao = new UserDao();
            var ex = dao.Login(username, PWMD5.MD5Hash(pw));
            string w = "";
            if (username != "" || pw != "")
            {
                if (ex == 1)
                {
                    if (username == "admin")
                    {
                        w = "ad";
                    }
                    else
                    {
                        w = "cl";
                    }
                    var user = dao.getName(username);
                    var userSession = new UserLogin();
                    userSession.Name = user.UserName;
                    userSession.ID = user.ID;
                    Session.Add(CommonStand.User_Session, userSession);
                    stt = true;
                }
                else if (ex == -1)
                {
                    stt = false;
                    s = "Tài khoản của bạn bị khóa!";
                }
                else if (ex == 0)
                {
                    stt = false;
                    s = "Tên tài khoản không tồn tại!";
                }
                else if (ex == -2)
                {
                    stt = false;
                    s = "Mật khẩu không đúng!";
                }
            }
            else
            {
                stt = false;
                s = "Không được bỏ trống!";
            }

            return Json(new { status = stt, noti = s, wh = w }, JsonRequestBehavior.AllowGet);
        }

        //login with facebook
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        //login with google
  
        public ActionResult LoginWithGooglePlus()
        {
            GoogleConnect.ClientId = "611807682848-prqem7hdaujmrtgn48vlvsbrkrpcmtbf.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "SXnBKRtGeD5iLpDte_FmoIwt";
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("profile", "email");
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                Tbl_User profile = new JavaScriptSerializer().Deserialize<Tbl_User>(json);

                string email = profile.Email;
                string userName = profile.UserName;
                string firstname = profile.Name;

                var user = new Tbl_User();
                user.Email = email;
                user.UserName = userName;
                user.Status = true;
                
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.Name = user.Name;
                    userSession.ID = user.ID;
                    Session.Add(CommonStand.User_Session, userSession);

                }
            }
            if (Request.QueryString["error"] == "access_denied")
            {
                return Content("access_denied");
            }
            return RedirectToAction("/");

        }
        //log out
        public ActionResult Logout()
        {
            Session[CommonStand.User_Session] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }


        //create user
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Wrong Captcha!")]
        public JsonResult Register(string name, string pw, string rpw, string phone, string email)
        {
            string userInput = Request.Params["CaptchaCode"] as string;
            string captchaId = Request.Params["ExampleCaptcha_CaptchaImage"] as string;
            string instanceId = Request.Params["ExampleCaptcha"] as string;

            bool isHuman = BotDetect.Web.Captcha.Validate(captchaId, userInput,
              instanceId);

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
            else if (dao.checkEmail(email))
            {
                s = " Email đã tồn tại!";
                stt = false;
            }
            //else if (isHuman == false)
            //{
            //    s = " Mã xác nhận không đúng!!";
            //    stt = false;
            //}
            else
            {
                var user = new Tbl_User();
                user.UserName = name;
                user.Password = PWMD5.MD5Hash(pw);
                user.Phone = phone;
                user.Email = email;
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
        public bool IsValCaptcha(string recaptcha)
        {
            if (string.IsNullOrEmpty(recaptcha))
            {
                return false;
            }
            var secreKey = "6LeiCvYUAAAAADP1GvevNmYcgInm3p_LZtIGJkXL";
            string remoteIP = Request.ServerVariables["REMOTE_ADDR"];
            string myPrameter = string.Format("secret={0},& responsive={1},remoteip={2}", secreKey, recaptcha, remoteIP);
            Recaptcha re;
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var json = wc.UploadString("https://www.google.com/recaptcha/api/siteverify", myPrameter);
                var js = new DataContractJsonSerializer(typeof(Recaptcha));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(json));
                re = js.ReadObject(ms) as Recaptcha;
                if (re != null && re.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}