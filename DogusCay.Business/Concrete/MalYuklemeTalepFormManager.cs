// DogusCay.Business.Concrete.MalYuklemeTalepFormManager.cs
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities; // Product entity'si için
using DogusCay.Entity.Entities.MalYuklemeTalep; // MalYuklemeTalepForm entity'si için
using DogusCay.Entity.Entities.Talep; // TalepTip ve TalepDurumu enumları için

// DTO'larınızın doğru namespace'i. Lütfen kendi projenizdeki gerçek konumu kontrol edin.
using DogusCay.DTO.DTOs.MalYuklemeDtos;

namespace DogusCay.Business.Concrete
{
    public class MalYuklemeTalepFormManager : GenericManager<MalYuklemeTalepForm>, IMalYuklemeTalepFormService
    {
        private readonly IMalYuklemeTalepFormRepository _malYuklemeTalepFormRepository;
        private readonly IProductService _productService; // Ürün servisi bağımlılığı

        // Constructor: Gerekli repository ve servisleri Dependency Injection ile alıyoruz
        public MalYuklemeTalepFormManager(
            IRepository<MalYuklemeTalepForm> repository, // Generic Manager için
            IMalYuklemeTalepFormRepository malYuklemeTalepFormRepository, // Özel MalYukleme Repository için
            IProductService productService) // Ürün servisi için
            : base(repository) // GenericManager'ın constructor'ını çağırıyoruz
        {
            _malYuklemeTalepFormRepository = malYuklemeTalepFormRepository;
            _productService = productService; // Ürün servisini atıyoruz
        }

        // Mal Yükleme Talep Formu oluşturma metodu (SENKRON hale getirildi)
        // Bu metot, frontend'den gelen CreateMalYuklemeTalepFormDto'yu işler
        // ve ürün detaylarını veritabanından çekerek formu oluşturur.
        public MalYuklemeTalepForm TCreateMalYuklemeTalepForm(CreateMalYuklemeTalepFormDto dto, int authenticatedUserId)
        {
            // Yeni MalYuklemeTalepForm entity'sini oluşturuyoruz
            var formEntity = new MalYuklemeTalepForm
            {
                AppUserId = authenticatedUserId, // Kimliği doğrulanmış kullanıcının ID'si Controller'dan geliyor
                KanalId = dto.KanalId,
                DistributorId = dto.DistributorId,
                PointGroupTypeId = dto.PointGroupTypeId,
                PointId = dto.PointId,
                TalepTip = TalepTip.MalYukleme, // Talep tipi sabit MalYukleme olarak ayarlanır
                TalepDurumu = TalepDurumu.Bekliyor, // Talep durumu başlangıçta Bekliyor olarak ayarlanır
                CreateDate = DateTime.Now, // Oluşturulma tarihi otomatik olarak atanır
                MalYuklemeTalepFormDetails = new List<MalYuklemeTalepFormDetail>() // Detay listesini başlatıyoruz
            };

            decimal brutTotal = 0;
            decimal toplamAgirlikKg = 0;

            foreach (var item in dto.MalYuklemeTalepFormDetails)
            {
                var product = _productService.TGetProductDetailsById(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Mal Yükleme Talep Formu detaylarında ProductId: {item.ProductId} bulunamadı.");

                // Kategori hiyerarşisi
                int? topCategoryId = product.Category?.ParentCategory?.ParentCategory?.CategoryId;
                int? middleCategoryId = product.Category?.ParentCategory?.CategoryId;
                int? lowestCategoryId = product.Category?.CategoryId;

                // Hesaplamalar
                decimal price = product.Price;
                int koliIciAdet = product.KoliIciAdet;
                int quantity = item.Quantity;
                decimal brutTutar = price * quantity;

                decimal discount1 = item.Discount1 ?? 0;
                decimal discount2 = item.Discount2 ?? 0;
                decimal fixedPrice = item.FixedPrice ?? 0;

                decimal discountedPrice1 = price * (1 - discount1 / 100m);
                decimal discountedPrice2 = discountedPrice1 * (1 - discount2 / 100m);
                decimal netTutar = discountedPrice2 * quantity;

                if (fixedPrice > 0)
                {
                    netTutar -= fixedPrice;
                    if (netTutar < 0) netTutar = 0;
                }

                decimal netAdetFiyat = (quantity > 0 && koliIciAdet > 0) ? netTutar / (quantity * koliIciAdet) : 0;
                decimal maliyet = (brutTutar > 0) ? 1 - (netTutar / brutTutar) : 0;

                var detail = new MalYuklemeTalepFormDetail
                {
                    ProductId = item.ProductId,
                    ProductName = product.ProductName,
                    ErpCode = product.ErpCode,
                    CategoryId = topCategoryId ?? 0,
                    SubCategoryId = middleCategoryId,
                    SubSubCategoryId = lowestCategoryId,
                    UnitTypeId = product.UnitTypeId,
                    ApproximateWeightKg = product.ApproximateWeightKg,
                    Price = price,
                    KoliIciAdet = koliIciAdet,
                    Quantity = quantity,

                    // Hesaplanan ve gelen alanlar
                    Discount1 = item.Discount1,
                    Discount2 = item.Discount2,
                    FixedPrice = item.FixedPrice,
                    BrutTutar = brutTutar,
                    NetTutar = netTutar,
                    NetAdetFiyat = netAdetFiyat,
                    Maliyet = maliyet
                };

                formEntity.MalYuklemeTalepFormDetails.Add(detail);
            }


            formEntity.BrutTotal = Math.Round(brutTotal, 2);
            // Not: Total ve diğer iskontolar backend'de hesaplanacaksa burada işlenmelidir.
            // Şu an için Total = BrutTotal olarak atanmıştır.
            formEntity.Total = formEntity.BrutTotal;
            formEntity.ToplamAgirlikKg = Math.Round(toplamAgirlikKg, 2);
            formEntity.Maliyet = 0; // Eğer Product'tan maliyet gelmiyorsa veya burada hesaplanmıyorsa

            // Form entity'sini veritabanına kaydetmek için GenericManager'ın senkron 'Create' metodunu kullanıyoruz
            base.TCreate(formEntity); // Artık 'await' ve 'Async' yok

            return formEntity; // Oluşturulan formu geri döndürüyoruz
        }

        // IMalYuklemeTalepFormService arayüzündeki diğer metotların implementasyonları:

        public List<MalYuklemeTalepForm> TGetAllWithUser()
        {
            return _malYuklemeTalepFormRepository.GetAllWithUser();
        }

        public List<MalYuklemeTalepForm> TGetAllByUserId(int userId)
        {
            return _malYuklemeTalepFormRepository.GetAllByUserId(userId);
        }

        public MalYuklemeTalepForm TGetDetailsForForm(int id)
        {
            return _malYuklemeTalepFormRepository.GetDetailsForForm(id);
        }

        public void TUpdateStatus(int talepFormId, TalepDurumu newStatus, int userId)
        {
            // Bu metodun implementasyonunu daha önce yapmıştık.
            var talepForm = _malYuklemeTalepFormRepository.GetById(talepFormId);
            if (talepForm != null)
            {
                talepForm.TalepDurumu = newStatus;
                talepForm.OnaylayanAdminId = userId;
                _malYuklemeTalepFormRepository.Update(talepForm);
            }
            else
            {
                throw new InvalidOperationException($"TalepForm with ID {talepFormId} not found for status update.");
            }
        }
        public List<ResultMalYuklemeTalepFormDto> TGetAllForIndex()
        {  
            return _malYuklemeTalepFormRepository.GetAllForIndex();
        }
    }
}
