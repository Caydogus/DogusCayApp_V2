using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        // Üst kategori ID'si (nullable)
        public int? ParentCategoryId { get; set; }

        // Navigasyon: Üst kategori
        public Category? ParentCategory { get; set; }

        // Navigasyon: Alt kategoriler
        public ICollection<Category> SubCategories { get; set; }

        // Navigasyon: Bu kategoriye ait ürünler
        public ICollection<Product> Products { get; set; }
    }
}
