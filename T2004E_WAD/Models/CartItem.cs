using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class CartItem //tao cartitem de lay so luong do trong product ko co soluong
    {
        public CartItem(Product product,int qty)
        {
            Product = product;
            Quantity = qty;
        }
        public Product Product{get;set; }
        public int Quantity { get; set; }
    }
}