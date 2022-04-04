using Model.Entity;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDao
    {
        AceSportDB db = null;
        public OrderDao()
        {
            db = new AceSportDB();
        }
        public long Insert(Tbl_Order order)
        {
            db.Tbl_Order.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        //order admin
        public IEnumerable<ViewModelOrder> getAllOrder(int page, int pagesize)
        {
            var model = (from a in db.Tbl_Order
                         join b in db.Tbl_OrderDetail
                         on a.ID equals b.OrderID
                         where a.ID == b.OrderID
                         select new
                         {
                             mdh = a.ID,
                             name = a.ShipName,
                             phone = a.ShipMobile,
                             Add = a.ShipAddress,
                             create = a.CreatedDate,
                             email = a.ShipEmail,
                             method = a.PaymentMethod,
                             pr = b.Price,
                             stt = a.Status
                         }).AsEnumerable().Select(x => new ViewModelOrder()
                         {
                             ID = x.mdh,
                             ShipName = x.name,
                             ShipMobile = x.phone,
                             ShipAddress = x.Add,
                             CreatedDate = x.create,
                             ShipEmail = x.email,
                             PaymentMethod = x.method,
                             Price = x.pr,
                             Status = x.stt
                         });
            model.OrderBy(x => x.ID);
            return model.ToPagedList(page, pagesize);
        }


        public List<ViewModelOrder> getAllOrderE()
        {
            var model = (from a in db.Tbl_Order
                         join b in db.Tbl_OrderDetail
                         on a.ID equals b.OrderID
                         join c in db.Tbl_Product
                         on b.ProductID equals c.ID
                         where a.ID == b.OrderID && b.ProductID==c.ID
                         select new
                         {
                             mdh = a.ID,
                             name = a.ShipName,
                             phone = a.ShipMobile,
                             Add = a.ShipAddress,
                             create = a.CreatedDate,
                             email = a.ShipEmail,
                             method = a.PaymentMethod,
                             namep=c.Name,
                             pr = b.Price,
                             qty=b.Quantity,
                             siz=b.Size,
                             stt = a.Status
                         }).AsEnumerable().Select(x => new ViewModelOrder()
                         {
                             ID = x.mdh,
                             ShipName = x.name,
                             ShipMobile = x.phone,
                             ShipAddress = x.Add,
                             CreatedDate = x.create,
                             ShipEmail = x.email,
                             PaymentMethod = x.method,
                             Name=x.namep,
                             Price = x.pr,
                             Quantity=x.qty,
                             Size=x.siz,
                             Status = x.stt
                         });
            model.OrderBy(x => x.ID);
            return model.ToList();
        }

        //order detail
        public Tbl_OrderDetail OrderDetails(long id)
        {
            return db.Tbl_OrderDetail.FirstOrDefault(x => x.OrderID == id);
        }

        //delete order
        public bool delete(long id)
        {

            try
            {
                var del = db.Tbl_Order.SingleOrDefault(x => x.ID == id);
                db.Tbl_Order.Remove(del);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //edit
        public bool EditOrder(long id, string name, string phone, string add)
        {
            try
            {
                var order = db.Tbl_Order.SingleOrDefault(x => x.ID == id);
                order.ShipName = name;
                order.ShipMobile = phone;
                order.ShipAddress = add;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //order detail
        public Tbl_Order getOrder(long id)
        {
            return db.Tbl_Order.SingleOrDefault(x => x.ID == id);
        }

        //total order
        public int TotalOrder()
        {
            var x = db.Tbl_Order.Count();
            return x;
        }

        //total order to day
        public int TotalOrderDay()
        {
            var x = db.Tbl_Order.Where(a => a.CreatedDate == DateTime.Now).Count();
            return x;
        }
        //statis  by month
        public int TotalPrice()
        {

            var lstDDH = db.Tbl_Order.Where(n => n.CreatedDate.Value.Month == DateTime.Now.Month && n.CreatedDate.Value.Year == DateTime.Now.Year);
            int x = 0;
            foreach (var item in lstDDH)
            {
                x += (item.Tbl_OrderDetail.Sum(n => n.Price));
            }
            return x;
        }

    }
}
