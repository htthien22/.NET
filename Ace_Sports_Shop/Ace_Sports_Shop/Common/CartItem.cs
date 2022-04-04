using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ace_Sports_Shop.Common
{
    [Serializable]
    public class CartItem
    {
        public Tbl_Product product { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}