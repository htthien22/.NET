using Ace_Sports_Shop.Common;
using Model.DAO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Ace_Sports_Shop.NganLuongAPI.API;

namespace Ace_Sports_Shop.Controllers
{
    public class CartController : Controller
    {
        //ngan luong
        private string merchantId = ConfigurationManager.AppSettings["MerchantId"].ToString();
        private string merchantPassword = ConfigurationManager.AppSettings["MerchantPassword"].ToString();
        private string merchantEmail = ConfigurationManager.AppSettings["MerchantEmail"].ToString();

        //sesssion
        private string cartSession = CommonStand.Cart_Session;
        // GET: Cart
        public ActionResult Index()
        {

            var cart = Session[cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.cartP = new ProductDAO().bestseller(4);
            return View(list);
        }
        //add to cart
        public JsonResult AddItem(long ProductID, string Size, string Color, int Quantity)
        {
            var product = new ProductDAO().Details(ProductID);
            var cart = Session[cartSession];


            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.ID == ProductID))
                {

                    foreach (var item in list)
                    {
                        //if exists count
                        if (item.product.ID == ProductID)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                }
                else
                {
                    //create new cart
                    var item = new CartItem();
                    item.product = product;
                    item.Quantity = Quantity;
                    item.Color = Color;
                    item.Size = Size;
                    list.Add(item);
                }
                Session[cartSession] = list;
            }
            else
            {
                try
                {
                    //create new cart
                    var item = new CartItem();
                    item.product = product;
                    item.Quantity = Quantity;
                    item.Color = Color;
                    item.Size = Size;
                    var list = new List<CartItem>();
                    list.Add(item);
                    Session[cartSession] = list;

                }
                catch (Exception ex)
                {

                }

            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        //edit cart
        public JsonResult UpdateQuantity(long ProductID, int Quantity)
        {

            var cart = Session[cartSession];
            var sessioncart = (List<CartItem>)cart;
            var list = (List<CartItem>)cart;
            if (cart != null)
            {
                if (list.Exists(x => x.product.ID == ProductID))
                {
                    foreach (var item in list)
                    {
                        if (item.product.ID == ProductID)
                        {
                            item.Quantity = Quantity;
                        }
                    }
                }

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        //delete cart
        public JsonResult Del(int ProductID)
        {

            var cart = Session[cartSession];
            var sessioncart = (List<CartItem>)cart;
            sessioncart.RemoveAll(x => x.product.ID == ProductID);
            cart = sessioncart;
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);

        }
        //checkout
        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = Session[cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        //payment
        public JsonResult Payment(string address, string mobile, string shipName, string email, string total, string check, string paymentMethod, string BankCode)
        {
            
            bool stt = false;
            var s = "";
            var order = new Tbl_Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            order.PaymentMethod = paymentMethod;
            order.Status = false;
            

            if (paymentMethod == "COD")
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[cartSession];
                var detailDao = new OrderDetailsDao();
                foreach (var item in cart)
                {
                    var orderDetail = new Tbl_OrderDetail();
                    orderDetail.ProductID = item.product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Color = item.Color;
                    orderDetail.Size = item.Size;
                    orderDetail.Price = item.product.PromotionPrice*item.Quantity;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    new ProductDAO().qty(item.product.ID, item.Quantity);
                }              
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/neworder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{date}}", DateTime.Now.ToString());
                content = content.Replace("{{paymentmethod}}", paymentMethod);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                if (string.IsNullOrEmpty(email))
                {
                    new SendMail().Send(toEmail, "Đơn hàng mới từ AceSport", content);

                }
                else
                {
                    new SendMail().Send(toEmail, "Đơn hàng mới từ AceSport", content);
                    new SendMail().Send(email, "Đơn hàng mới từ AceSport", content);
                }
                new SendMail().Send(toEmail, "Đơn hàng mới từ AceSport", content);
                stt = true;
                s = "Thanh toán thành công!";
                Session[cartSession] = null;

            }
            else
            {
                var currentLink = ConfigurationManager.AppSettings["CurrentLink"].ToString();
                RequestInfo info = new RequestInfo();
                info.Merchant_id = merchantId;
                info.Merchant_password = merchantPassword;
                info.Receiver_email = merchantEmail;

                info.cur_code = "vnd";
                info.bank_code = BankCode;

                info.Order_code = order.ID.ToString();
                info.Total_amount = total;
                info.fee_shipping = "0";
                info.Discount_amount = "0";
                info.order_description = "Thanh toán đơn hàng tại ACeSoprt";
                info.return_url = currentLink + "xac-nhan-don-hang";
                info.cancel_url = currentLink + "huy-don-hang";

                info.Buyer_fullname = order.ShipName;
                info.Buyer_email = order.ShipEmail;
                info.Buyer_mobile = order.ShipMobile;

                APICheckoutV3 objNLChecout = new APICheckoutV3();
                ResponseInfo result = objNLChecout.GetUrlCheckout(info, paymentMethod);
                if (result.Error_code == "00")
                {
                    return Json(new
                    {
                        status = true,
                        urlCheckout = result.Checkout_url,
                        message = result.Description
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        s = result.Description
                    });
                }

            }
            
            return Json(new { status = stt, noti = s }, JsonRequestBehavior.AllowGet);
        }
        //xac nhan don hang
        //public ActionResult ConfirmOrder()
        //{
        //    string token = Request["token"];
        //    RequestCheckOrder info = new RequestCheckOrder();
        //    info.Merchant_id = merchantId;
        //    info.Merchant_password = merchantPassword;
        //    info.Token = token;
        //    APICheckoutV3 objNLChecout = new APICheckoutV3();
        //    ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
        //    if (result.errorCode == "00")
        //    {
        //        //update status order
        //        _orderService.UpdateStatus(int.Parse(result.order_code));
        //        _orderService.Save();
        //        ViewBag.IsSuccess = true;
        //        ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
        //    }
        //    else
        //    {
        //        ViewBag.IsSuccess = true;
        //        ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
        //    }
        //    return View();
        //}
        //huy don hang
        public ActionResult CancelOrder()
        {
            return View();
        }


    }
}