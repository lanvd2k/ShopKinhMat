using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanDo.Models
{
    public class CartItem
    {
        public modelProduct product { get; set; }

        public int quantity { get; set; }

        public int countCart { get; set; }

        public string meThod { get; set; }

        public long priceTotal { get; set; }
    }
}