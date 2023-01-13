﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }    
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }  
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}