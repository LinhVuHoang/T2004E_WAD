using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class Cart
    {
        private Customer customer;
        private List<CartItem> cartItems;//tạo list cartitem để lưu thông tin danh sách sản phẩm mua
        private decimal grandTotal; //tạo biến để lưu tổng giá tiền
        public Cart()//hàm khởi tạo
        {
            cartItems = new List<CartItem>();// khởi tạo list cartitem
            }
        public List<CartItem> CartItems { get => cartItems; }// truy vấn để lây dữ liệu trong list cartitem.cs
        public decimal GrandTotal { get => grandTotal; set => grandTotal = value; }//set get dữ liệu grandtotal
        public Customer Customer { get => customer; set => customer = value; } // set get dữ liệu customer
        public CartItem this[int index]// Một indexer trong C# cho phép một
                                       //đối tượng để được lập chỉ mục, ví dụ như
                                       //một mảng. Khi bạn định nghĩa một indexer
                                       //cho một lớp, thì lớp này vận hành tương
                                       //tự như một virtual array (mảng ảo).
                                       //Sau đó, bạn có thể truy cập instance
                                       //(sự thể hiện) của lớp này bởi sử
                                       //dụng toán tử truy cập mảng trong
                                       //C# là ([ ]). 
        {
            get => CartItems[index];
            set => CartItems[index] = value;
        }
        public bool AddToCart(CartItem item)//thêm vào cart
        {
            //kiem tra xem co san pham chua
            int check = CheckExists(item);
            if(check >= 0) //có sản phẩm
            {
                CartItems[check].Quantity += item.Quantity;//tăng số lượng
            }
            else
            {
                CartItems.Add(item);// thêm item vài list cartiem
            }
            CalculateGrandTotal();//gọi hàm tính tiền
            return true;
        }
        public void RemoveItem(int id)
        {
            // hàm xóa cart
            foreach(var item in CartItems) // biến var chương trình sẽ tự ép kiểu cho biến
            {
                if(item.Product.Id == id) // check id truyền vào có đúng id trong product cần remove
                {
                    CartItems.Remove(item); // xóa item đã gọi
                    CalculateGrandTotal(); // tính tiền lại
                    return;
                }
            }
        }
        public int CheckExists(CartItem item) // hàm check xem có tồn tại item ko
        {
            for(int i = 0; i < CartItems.Count; i++)
            {
                if(CartItems[i].Product.Id == item.Product.Id) // kiếm tra id trong list cartitem có bằng id truyền vào
                {
                    return i;
                }
            }
            return -1; // ko có trả về -1 vì ko có product cần tìm
        }
        public void CalculateGrandTotal()//tính tổng tiền sản phẩm
        {
            decimal grand = 0;
            foreach(CartItem item in CartItems){
                grand += item.Product.Price * item.Quantity;
            }
            grandTotal = grand;
            }
       
    }
}