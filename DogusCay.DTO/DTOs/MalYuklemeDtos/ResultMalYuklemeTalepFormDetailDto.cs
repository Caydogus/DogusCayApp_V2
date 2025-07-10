using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.MalYuklemeDtos
{
    public class ResultMalYuklemeTalepFormDetailDto
    {
        public int ResultMalYuklemeTalepFormDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ErpCode { get; set; }

        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubSubCategoryId { get; set; }

        // <<-- KRİTİK: Bu property'ler, Repository'den gelecek kategori isimlerini tutacak -->>
        public string? CategoryName { get; set; }    // Ana kategori adı (Demlik, Bardak, Dökme, BitkiMeyve)
        public string? SubCategoryName { get; set; } // Bir üst kategori adı (örn: BLACK LABEL)
        public string? SubSubCategoryName { get; set; } // En alt kategori adı (örn: BLACK LABEL S...)
        // <<------------------------------------------------------------------------------->>

        public int UnitTypeId { get; set; }
        public decimal ApproximateWeightKg { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public int Quantity { get; set; }

        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? NetTutar { get; set; }
        public decimal? NetAdetFiyat { get; set; }
        public decimal? BrutTutar { get; set; }
        public decimal? Maliyet { get; set; }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DogusCay.Entity.Entities;

//namespace DogusCay.DTO.DTOs.MalYuklemeDtos
//{
//    public class ResultMalYuklemeTalepFormDetailDto
//    {
//        public int ResultMalYuklemeTalepFormDetailId { get; set; }
//        public int ProductId { get; set; }
//        public Product Product { get; set; } // <<-- BURAYA EKLEYİN: Product için navigation property

//        public string ProductName { get; set; }
//        public string ErpCode { get; set; }
//        public int CategoryId { get; set; }
//        public int? SubCategoryId { get; set; }
//        public Category SubCategory { get; set; } // <<-- BURAYA EKLEYİN: Alt kategori için navigation property

//        public int? SubSubCategoryId { get; set; }
//        public int UnitTypeId { get; set; }
//        public decimal ApproximateWeightKg { get; set; }
//        public decimal Price { get; set; }
//        public int KoliIciAdet { get; set; }
//        public int Quantity { get; set; }

//        public decimal? Discount1 { get; set; }      // İskonto 1 (%)
//        public decimal? Discount2 { get; set; }      // İskonto 2 (%)
//        public decimal? FixedPrice { get; set; }     // Sabit Bedel
//        public decimal? NetTutar { get; set; }       // Hesaplanan Net Tutar
//        public decimal? NetAdetFiyat { get; set; }   // Hesaplanan Net Adet Fiyatı
//        public decimal? BrutTutar { get; set; }      // Hesaplanan Brüt Tutar
//        public decimal? Maliyet { get; set; }        // Hesaplanan Maliyet (%)


//    }

//}

