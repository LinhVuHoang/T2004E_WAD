using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_WAD.Models
{
    public class Customer //tao customer de lay thong tin khach hang
    {
        public Customer(string name,string telephone,string address)
        {

        }
        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Address { get; set; }
    }
}