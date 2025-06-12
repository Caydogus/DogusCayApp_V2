using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.WebUI.DTOs.TalepDtos
{
    public class CreateTalepFormDto
    {
        // Zincir alanları
        public int KanalId { get; set; }
        public int? DistributorId { get; set; }
        public int? PointGroupTypeId { get; set; } // opsiyonel, eğer nokta grubu tipi seçilirse
        public int? PointId { get; set; }

        // Ürün bilgileri
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubSubCategoryId { get; set; }


        public int ProductId { get; set; }
        public string ProductName { get; set; } // UI için
        public string ErpCode { get; set; } // opsiyonel
        public decimal ApproximateWeightKg { get; set; } // opsiyonel
        public int KoliIciAdet { get; set; } // opsiyonel
        public decimal? SabitBedelTL { get; set; }
        // Talep detayları
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }

        public decimal KoliToplamAgirligiKg { get; set; }
        public int KoliIciToplamAdet { get; set; }
        public decimal ListeFiyat { get; set; }
        public decimal SonAdetFiyati { get; set; }
        public decimal AdetFarkDonusuTL { get; set; }
        // public decimal AdetFarkDonusuYuzde { get; set; }

        public DateTime ValidFrom { get; set; } = DateTime.Today;
        public DateTime ValidTo { get; set; } = DateTime.Today.AddDays(7);

        public string Note { get; set; }

        public int? AppUserId { get; set; }

        // Hesaplanan alanlar
        public decimal Total { get; set; }
        public decimal BrutTotal { get; set; }
        public decimal Maliyet { get; set; }


    }
}
