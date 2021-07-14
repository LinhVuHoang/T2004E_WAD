using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace T2004E_WAD.Models
{
    public class Category
    {
        [Key] // khoa chinh
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        public string NameC { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả danh mục")]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }
    }
}