using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T2004E_WAD.Context;
using T2004E_WAD.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
namespace T2004E_WAD.Controllers
{
    public class UserController : Controller
    {
        private DataContext dataContext = new DataContext(); // gọi datacontext để kết nối database
        // GET: User
       [Authorize] //tạo login cho trang đăng nhập với auto cùng liên kết link trong web.config
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register() // hành động đăng ký
        {
            return View();
        }
        [HttpPost]//phương thức đăng lên
        [ValidateAntiForgeryToken] //tạo token để đăng lên lên
        public ActionResult Register(User user)//truyền biến user để cho vào đăng ký
        {
            if(ModelState.IsValid)// hàm kiểm tra lỗi của wcp nếu ko lỗi thì chạy
            {
                var check = dataContext.Users.FirstOrDefault(s => s.Email == user.Email); // check xem có trùng với email đã đc dky ko
                if(check == null)
                {
                    //chưa có tài khoản này
                    user.Password = GetMD5(user.Password);//Mã hóa password theo GetMD5(string std)
                    dataContext.Users.Add(user); //add vào dtb
                    dataContext.Configuration.ValidateOnSaveEnabled = false;
                    dataContext.SaveChanges();//lưu sự thay đổi
                    return RedirectToAction("Index");//trả về View index
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    //ViewBag sử dụng kiểu động (dynamic) mà chúng ta đã có trong phiên bản C# 4.0. Nó là một vỏ bọc của ViewData và
                    //cung cấp thuộc tính động cho ViewData. để báo lỗi 
                }
            }    
                return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();//tạo biến md5 của service để mã hóa password
            byte[] frData = Encoding.UTF8.GetBytes(str);//tạo mảng byte để lấy chuỗi string
            byte[] tgData = md5.ComputeHash(frData);//tạo mảng byte để mã hõa mảng byte chữa password tăng tính bảo mật
            string hashString = ""; //tạo biến để + ra chuỗi đưa vào database
            for(int i = 0; i < tgData.Length; i++)
            {
                hashString += tgData[i].ToString("x2");//chuyển thành chuỗi string nhị phân
            }
            return hashString;
        }
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl; // gán cho returnurl trong đường link hiện trên ứng dụng
            //làm login
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email,string password)
        {
            
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = dataContext.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if(data.Count > 0)
                {
                    var u = data.FirstOrDefault();
                    //login thanh cong
                    FormsAuthentication.SetAuthCookie(u.FullName, true);//Cookie cũng được dùng để lưu lại các thông tin khác mà người dùng nhập
                                                                        //hay điền vào trang web như tên, địa chỉ, mật khẩu, v.v...
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

    }
}