﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CapaAdmin.Models
{
    public class Product
    {

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } ="";

        [MaxLength(100)]
        public string Brand { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";

        [Precision(16,2)]
        public decimal Price { get; set; }

        [Precision(16, 2)]
        public decimal Stock { get; set; }


        [Precision(16, 2)]
        public decimal CodeProduct { get; set; }

        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }


            
    }
}
