using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameB { get; set; }
   
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
    }
}