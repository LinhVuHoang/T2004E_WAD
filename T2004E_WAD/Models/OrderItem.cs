using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class OrderItem //sản phẩm đc order
    {
        [Key]
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
    }
}