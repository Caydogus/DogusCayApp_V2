using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogusCay.Entity.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }  
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }  
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public bool IsShown { get; set; } = true;
    }
}
