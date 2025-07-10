using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities.MalYuklemeTalep; 
using DogusCay.Entity.Entities.Talep; 
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class MalYuklemeTalepFormManager : GenericManager<MalYuklemeTalepForm>, IMalYuklemeTalepFormService
    {
        private readonly IMalYuklemeTalepFormRepository _malYuklemeTalepFormRepository;
        private readonly IProductService _productService;
        public MalYuklemeTalepFormManager(
            IRepository<MalYuklemeTalepForm> repository, 
            IMalYuklemeTalepFormRepository malYuklemeTalepFormRepository,
            IProductService productService)
            : base(repository) 
        {
            _malYuklemeTalepFormRepository = malYuklemeTalepFormRepository;
            _productService = productService; 
        }
        public MalYuklemeTalepForm TCreateMalYuklemeTalepForm(CreateMalYuklemeTalepFormDto dto, int authenticatedUserId)
        {
            var formEntity = new MalYuklemeTalepForm
            {
                AppUserId = authenticatedUserId,
                KanalId = dto.KanalId,
                DistributorId = dto.DistributorId,
                PointGroupTypeId = dto.PointGroupTypeId,
                PointId = dto.PointId,
                TalepTip = TalepTip.MalYukleme,
                TalepDurumu = TalepDurumu.Bekliyor,
                CreateDate = DateTime.Now,
                MalYuklemeTalepFormDetails = new List<MalYuklemeTalepFormDetail>()
            };

            decimal brutTotal = 0;
            decimal toplamAgirlikKg = 0;
            decimal totalNet = 0;

            foreach (var item in dto.MalYuklemeTalepFormDetails)
            {
                var product = _productService.TGetProductDetailsById(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Mal Yükleme Talep Formu detaylarında ProductId: {item.ProductId} bulunamadı.");

                int koliIciAdet = product.KoliIciAdet;
                int quantity = item.Quantity;
                decimal price = product.Price;
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
                decimal maliyet = (brutTutar > 0) ? Math.Round((1 - (netTutar / brutTutar)) * 100m, 2) : 0;

                // --- TOPLAM HESAPLAMALAR ---
                brutTotal += brutTutar;
                totalNet += netTutar;
                toplamAgirlikKg += (product.ApproximateWeightKg * quantity);

                var detail = new MalYuklemeTalepFormDetail
                {
                    ProductId = item.ProductId,
                    ProductName = product.ProductName,
                    ErpCode = product.ErpCode,
                    CategoryId = product.Category?.ParentCategory?.ParentCategory?.CategoryId ?? 0,
                    SubCategoryId = product.Category?.ParentCategory?.CategoryId,
                    SubSubCategoryId = product.Category?.CategoryId,
                    UnitTypeId = product.UnitTypeId,
                    ApproximateWeightKg = product.ApproximateWeightKg,
                    Price = price,
                    KoliIciAdet = koliIciAdet,
                    Quantity = quantity,
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
            formEntity.Total = Math.Round(totalNet, 2);
            formEntity.ToplamAgirlikKg = Math.Round(toplamAgirlikKg, 2);
            formEntity.Maliyet = (brutTotal > 0) ? Math.Round((1 - (totalNet / brutTotal)) * 100m, 2) : 0;

            base.TCreate(formEntity);

            return formEntity;
        }

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
