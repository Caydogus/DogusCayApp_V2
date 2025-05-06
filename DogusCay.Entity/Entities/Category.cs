using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogusCay.Entity.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }  // Kategorinin benzersiz kimliği

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }  // Kategorinin adı

        // Ana kategori için ParentCategoryId null olur, alt kategoriler için üst kategori ID'si burada tutulur
        public int? ParentCategoryId { get; set; }

        // Navigasyon özelliği: Eğer kategori bir alt kategori ise, üst kategoriye erişim sağlar
        public Category ParentCategory { get; set; }

        // Alt kategoriler (Çocuk kategoriler): Bir kategori birden fazla alt kategoriye sahip olabilir
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public ICollection<Product> Products { get; set; } = new List<Product>();


        // Kategorinin gösterilip gösterilmeyeceği bilgisini tutar (isteğe bağlı, admin paneli için)
        public bool IsShown { get; set; } = true;
    }
}
