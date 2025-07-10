using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.Entity.Entities.MalYuklemeTalep
{
    public class MalYuklemeTalepFormDetail
    {
        public int MalYuklemeTalepFormDetailId { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; } 

        public int? SubSubCategoryId { get; set; }
        public Category SubSubCategory { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } 
        public string? ProductName { get; set; }

        public string? ErpCode { get; set; }
        public int UnitTypeId { get; set; }
        public decimal ApproximateWeightKg { get; set; }
        public decimal Price { get; set; }
        public int KoliIciAdet { get; set; }
        public int Quantity { get; set; }

        // Yeni eklenen alanlar
        public decimal? Discount1 { get; set; }      // İskonto 1 (%)
        public decimal? Discount2 { get; set; }      // İskonto 2 (%)
        public decimal? FixedPrice { get; set; }     // Sabit Bedel
        public decimal? NetTutar { get; set; }       // Hesaplanan Net Tutar
        public decimal? NetAdetFiyat { get; set; }   // Hesaplanan Net Adet Fiyatı
        public decimal? BrutTutar { get; set; }      // Hesaplanan Brüt Tutar
        public decimal? Maliyet { get; set; }        // Hesaplanan Maliyet (%)

        public int MalYuklemeTalepFormId { get; set; }
        public MalYuklemeTalepForm MalYuklemeTalepForm { get; set; }
    }

}
