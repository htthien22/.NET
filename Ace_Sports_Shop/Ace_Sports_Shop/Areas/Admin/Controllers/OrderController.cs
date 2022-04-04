using Model.DAO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ace_Sports_Shop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order(int page = 1, int pagesize = 10)
        {
            var dao = new OrderDao().getAllOrder(page, pagesize);
            return View(dao);

        }


        //order details
        [HttpPost]
        public JsonResult Details(long ID)
        {
            bool stt = true;

            var dao = new OrderDao().OrderDetails(ID);
            string id = dao.OrderID.ToString();
            string price = dao.Price.ToString();
            string size = dao.Size.ToString();
            string color = dao.Color.ToString();
            string qty = dao.Quantity.ToString();

            return Json(new { status = stt, ID = id, Price = price, Size = size, Color = color, Qty = qty }, JsonRequestBehavior.AllowGet);
        }

        //delete

        public JsonResult Delete(long ID)
        {
            bool stt = true;
            new OrderDao().delete(ID);
            return Json(new { status = stt }, JsonRequestBehavior.AllowGet);
        }


        //edit order get
        public JsonResult GetOrder(long ID)
        {
            bool stt = true;
            var dao = new OrderDao().getOrder(ID);
            string id = dao.ID.ToString();
            string name = dao.ShipName.ToString();
            string phone = dao.ShipMobile.ToString();
            string add = dao.ShipAddress.ToString();

            return Json(new { status = stt, ID = id, Name = name, Phone = phone, Add = add }, JsonRequestBehavior.AllowGet);
        }
        //edit orrder
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

        //xuat excel
        public ActionResult ExcelExport()
        {

            var dao = new OrderDao().getAllOrderE();


            try
            {

                DataTable Dt = new DataTable();

                Dt.Columns.Add("Mã đơn hàng", typeof(string));
                Dt.Columns.Add("Tên sản phẩm", typeof(string));
                Dt.Columns.Add("Số lượng", typeof(string));
                Dt.Columns.Add("Thành tiền", typeof(string));
                Dt.Columns.Add("Tên người nhận", typeof(string));
                Dt.Columns.Add("SĐT", typeof(string));
                Dt.Columns.Add("Địa chỉ giao hàng", typeof(string));          
                Dt.Columns.Add("Phương thức thanh toán", typeof(string));
                Dt.Columns.Add("Trạng thái", typeof(string));
                Dt.Columns.Add("Ngày đặt", typeof(string));

                foreach (var data in dao)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.ID;
                    row[1] = data.Name +" X "+ data.Size;
                    row[2] = data.Quantity;
                    row[3] = data.Price;
                    row[4] = data.ShipName;
                    row[5] = data.ShipMobile;
                    row[6] = data.ShipAddress;
                    row[7] = data.PaymentMethod;
                    if (data.Status == true)
                    {
                        row[8] = "Đã giao hàng";
                    }
                    else
                    {
                        row[8] = "Chưa giao hàng";
                    }
                    row[9] = data.CreatedDate;
                    Dt.Rows.Add(row);

                }

                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Đơn hàng");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.Light9);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    //worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    //worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult Download()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                var x = "ĐơnHang"+ DateTime.Now.Day+ DateTime.Now.Month+ DateTime.Now.Year + ".xlsx";
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", x);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}