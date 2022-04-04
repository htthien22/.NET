using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class UserDao
    {
        AceSportDB db = null;
        public UserDao()
        {
            db = new AceSportDB();
        }
        public IEnumerable<Tbl_User> getALL(int page, int pagesize)
        {
            return db.Tbl_User.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
        }
        public long Insert(Tbl_User entity)
        {
            db.Tbl_User.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Tbl_User getName(string TenNguoiDung)
        {
            return db.Tbl_User.SingleOrDefault(x => x.UserName == TenNguoiDung);
        }
        public int Login(string TenNguoiDung, string MatKhau)
        {
            var ex = db.Tbl_User.SingleOrDefault(x => x.UserName == TenNguoiDung);
            if (ex == null)
            {
                return 0;
            }
            else
            {
                if (ex.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (ex.Password == MatKhau)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }

        }

        //login witg facebook
        public long InsertForFacebook(Tbl_User entity)
        {
            var user = db.Tbl_User.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Tbl_User.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }

        }

        //delete user
        public bool delete(long id)
        {

            try
            {
                var del = db.Tbl_User.SingleOrDefault(x => x.ID == id);
                db.Tbl_User.Remove(del);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //get user
        
        public Tbl_User getUser(long id)
        {
            return db.Tbl_User.SingleOrDefault(x => x.ID == id);
        }

        //edit
        public bool editUse(string user, string name,string pw, string phone, string add, bool stt)
        {
            try
            {
                var u = db.Tbl_User.SingleOrDefault(x => x.UserName == user);
                u.Name = name;
                u.Password = pw;
                u.Phone = phone;
                u.Address = add;
                u.Status = stt;
                u.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //check user register
        public bool checkName(string name)
        {
            return db.Tbl_User.Count(x => x.UserName == name) > 0;
        }

        public bool checkEmail(string email)
        {
            return db.Tbl_User.Count(x => x.Email == email) > 0;
        }
    }
}
