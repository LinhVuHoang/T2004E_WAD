using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string NameB { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
    }
}