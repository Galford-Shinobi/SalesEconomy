﻿using System.ComponentModel.DataAnnotations;

namespace SalesEconomy.Shared.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; } = null!;

        public int ProductId { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; } = null!;
    }
}
