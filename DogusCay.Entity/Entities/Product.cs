
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogusCay.Entity.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; }
        public string ErpCode { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }
        public decimal ApproximateWeightKg { get; set; }
        public bool IsShown { get; set; } = true;
        public decimal Price { get; set; }            
        public int KoliIciAdet { get; set; }
        public decimal? OneriRafFiyati { get; set; }//23.07.2025 eklendi
        public decimal? OneriAksiyonFiyati { get; set; }//23.07.2025 eklendi
        public ICollection<Sale> Sales { get; set; }
    }
}
