using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using T2004E_WAD.Context;
using T2004E_WAD.Models;
using System.Dynamic;
namespace T2004E_WAD.Controllers
{
    public class ProductController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Product
        public ActionResult Index(string search, string sortOrder, string categoryId)
        {//cach 1: dung thang dbset-- su dung duoc relationship
            // các cách để tạo search product trình bày theo thứ tự alpha b tăng dần hay giảm dần
         //var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            ViewBag.CategoryId = 0;
            string sort = !String.IsNullOrEmpty(sortOrder) ? sortOrder : "asc";
            //   if (!String.IsNullOrEmpty(search))
            //   {
            ////       var products = db.Products.Where(p => p.NameP.Contains(search)).Include(p=>p.Brand).OrderBy(p=>p.NameP).ToList();//orderby asc orderbydescening desc
            //         return View(products);
            //     }

            //     return View(db.Products.OrderBy(p=>p.NameP).ToList());   
            //cach 2 dung db raw -- tim trong mot bảng
            var products = from p in db.Products select p;
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.NameP.Contains(search)).Include(p => p.Category).Include(p => p.Brand);


            }
            switch (sort)
            {
                case "asc": products = products.OrderBy(p => p.NameP).Include(p => p.Category).Include(p => p.Brand); break;
                case "desc": products = products.OrderByDescending(p => p.NameP).Include(p => p.Category).Include(p => p.Brand); break;
            }
            if (!String.IsNullOrEmpty(categoryId))
            {
                var catId = Convert.ToInt32(categoryId);
                products = products.Where(p => p.CategoryID == catId);
                ViewBag.CategoryId = catId;
            }
            var categories = db.Categories.ToList();
            dynamic data = new ExpandoObject();
            data.Products = products;
            data.Categories = categories;
            //loc theo category products =products.where(p => p.CategoryID ==1);
            return View(data);

        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult AddToCart(int? id, int? qty) // hàm thêm product vào cart
        {
            try {
                Product product = db.Products.Find(id); // tạo biến theo product
                if(product == null)
                {
                    return HttpNotFound();
                }
                //them vao gio hang
                CartItem item = new CartItem(product, (int)qty); // tạo biến item theo cartitem
                //lay gio hang tu SessSion
                Cart cart = (Cart)Session["Cart"]; // lưu thông tin giỏ hàng
                if (cart == null)
                {
                    Customer customer = new Customer("Nguyễn Văn An", "0987654321", "Số 8 Tôn Thất Thuyết");
                    cart = new Cart();
                    cart.Customer = customer;
                }
                cart.AddToCart(item);
                Session["cart"] = cart;//thêm session
            }catch(Exception e)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult RemoveItem(int? id)
        {
            try
            {
                Cart cart = (Cart)Session["Cart"]; // ép kiểu Cart. Session là một từ phổ biến thường được dùng
                                                   // trong ngôn ngữ lập trình viên Website, Còn có kết nối với cơ sở dữ liệu
                                                   // như Database. Điều đặc biệt ở đây các chức năng đăng xuất, đăng nhập
                                                   // người dùng sẽ rất khó mà thực hiện nổi, nếu không sử dụng session
                                                   // tạo 1 tập tin lưu dữ liệu của Cart
                                                   //lúc đầu chưa có dữ liệu với Session Cart sau gán biến cart của Cart.cs để làm dữ liệu session mới
                if (cart == null)
                {
                    return HttpNotFound();

                }
                cart.RemoveItem((int)id);
                Session["cart"] = cart;//thêm session mới tên cart với biển cart có dữ liệu mới tạo ra 1 session mới 
            }catch(Exception e)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Cart");
        }
        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "NameB");
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "NameC");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameP,Image,Price,Description,CategoryID,BrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "Id", "NameB", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "NameC", product.CategoryID);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "NameB", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "NameC", product.CategoryID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameP,Image,Price,Description,CategoryID,BrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "NameB", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "NameC", product.CategoryID);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult CheckOut()
        {
            return View();//trả về View của các controller
        }
        [HttpPost]//phương thức đẩy dữ liệu lên của wcp
        public ActionResult CheckOut(Order order) // tạo biến tên order của Order.cs
        {
            if (ModelState.IsValid) // hàm kiểm tra lỗi của wcp nếu ko lỗi thì chạy
            {
                var cart = (Cart)Session["cart"];//truyền session lưu dữ liệu của cart sang order
                order.GrandTotal = cart.GrandTotal;
                order.CreatedAt = DateTime.Now;
                order.Status = 1;
                db.Orders.Add(order);
                db.SaveChanges();
                foreach(var item in cart.CartItems){
                    OrderItem orderItem = new OrderItem() { OrderID = order.Id, ProductID = item.Product.Id, Qty = item.Quantity, Price = item.Product.Price };
                    //tạo vòng lặp để tìm sản phẩm được order sau đó tạo biến của OderItem.cs để lưu vào
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges(); //lưu sự thay đổi dữ liệu
                Session["cart"] = null; // xóa giỏ hàng sau đi đặt được hàng
            }
            return RedirectToAction("CheckOutSuccess");//trả về public string Checkoutsuccess
        }
        public string CheckOutSuccess()
        {
            return "Tạo đơn thành công . Xin cảm ơn và hẹn gặp lại quý khách...";

        }
    }
}
