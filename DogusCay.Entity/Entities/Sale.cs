using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }

        // Satılan ürün
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Satış noktası (market, şube vs.)
        public int PointId { get; set; }
        public Point Point { get; set; }

        // Satışı yapan kişi (müdür ya da satışçı)
        public int UserId { get; set; }
        public AppUser User { get; set; }

        // Satış tarihi
        public DateTime SaleDate { get; set; }

        // Satış adedi (kilo veya koli olacak, ürünün birimine göre)
        public int Quantity { get; set; }

        // Anlaşma fiyatı (birim fiyat)
        public decimal UnitPrice { get; set; }

        // İndirim oranı (yüzde olarak, örn: %10 = 10)
        public decimal DiscountPercentage { get; set; }

        // İskonto altı yüzdesi (ayrı takip edilsin istedin)
        public decimal IskontoAltiPercentage { get; set; }

        // Hesaplanan net satış fiyatı
        public decimal NetPrice { get; set; }

        // Toplam net satış (NetPrice * Quantity)
        public decimal TotalNetPrice { get; set; }

        // Ödeme tipi (Nakit/Vadeli vs.)
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        // Satış iptal edildi mi?
        public bool IsCancelled { get; set; } = false;

        // Açıklama, not gibi alanlar
        public string? Note { get; set; }
      

    }

}
