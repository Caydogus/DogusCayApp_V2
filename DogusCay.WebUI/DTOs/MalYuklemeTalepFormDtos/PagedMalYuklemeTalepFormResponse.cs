namespace DogusCay.WebUI.DTOs.MalYuklemeTalepFormDtos
{
    public class PagedMalYuklemeTalepFormResponse
    {
        // Sayfanın verilerini tutan liste
        public List<ResultMalYuklemeTalepFormDto> Data { get; set; }

        // Toplam kayıt sayısı
        public int TotalCount { get; set; }

        // Mevcut sayfa numarası
        public int Page { get; set; }

        // Sayfa başına düşen kayıt sayısı
        public int PageSize { get; set; }
    }
}