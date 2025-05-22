using System;

namespace DogusCay.Entity.Entities.Talep
{
    public class TalepFormItem
    {
        public int TalepFormItemId { get; set; }

        public int TalepFormId { get; set; }
        public TalepForm TalepForm { get; set; }

        // Kategori - AltKategori opsiyonel
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? SubCategoryId { get; set; }
        public Category? SubCategory { get; set; }

        // Ürün
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Talep edilen miktar
        public int Quantity { get; set; }

        // Otomatik gelen ve form anında sabitlenen bilgiler
        public decimal Price { get; set; }         // Birim fiyat
        public int KoliIciAdet { get; set; }       // 1 kolideki adet
        public decimal KoliFiyati { get; set; }    // Hesaplanmış: Price * KoliIciAdet
        public decimal KoliAgirligiKg { get; set; } // Yaklaşık koli ağırlığı

        public decimal Iskonto1 { get; set; }
        public decimal Iskonto2 { get; set; }
        public decimal Iskonto3 { get; set; }
        public decimal Iskonto4 { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
