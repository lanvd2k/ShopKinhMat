namespace ShopBanDo.Models
{
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


    [Table("Products")]
    public class modelProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CatId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Img { get; set; }
        public string Detail { get; set; }
        public int Number { get; set; }
        public int Price { get; set; }
        public int Price_sale { get; set; }

        public string Metakey { get; set; }

        public string Metadesc { get; set; }

        public DateTime? Created_at { get; set; }

        public int? Created_by { get; set; }

        public DateTime? Updated_at { get; set; }

        public int? Updated_by { get; set; }

        public int Status { get; set; }

        public int? Discount { get; set; }
    }
}