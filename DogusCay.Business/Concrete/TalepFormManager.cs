using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Concrete
{
    // TalepForm ile ilgili iş mantığını yöneten servis katmanı
    public class TalepFormManager : GenericManager<TalepForm>, ITalepFormService
    {
        private readonly ITalepFormRepository _talepFormRepository;
        private readonly IMapper _mapper;
        public TalepFormManager(IRepository<TalepForm> repository, ITalepFormRepository talepFormRepository, IMapper mapper)
            : base(repository)
        {
            _mapper = mapper;
            _talepFormRepository = talepFormRepository;
        }

        public TalepForm CreateTalepFormWithCalculations(CreateTalepFormDto dto, int appUserId)
        {
            var entity = _mapper.Map<TalepForm>(dto);
            entity.AppUserId = appUserId;
            entity.Quantity = dto.Quantity <= 0 ? 1 : dto.Quantity;
            entity.PointId = dto.PointId ?? 0;
            entity.ValidFrom = dto.ValidFrom < new DateTime(1753, 1, 1) ? DateTime.Now : dto.ValidFrom;
            entity.ValidTo = dto.ValidTo < new DateTime(1753, 1, 1) ? DateTime.Now.AddDays(7) : dto.ValidTo;
            entity.TalepTip = TalepTip.Insert;

            // Hesaplamalar
            entity.BrutTotal = entity.Quantity * dto.Price;
            entity.KoliIciToplamAdet = entity.Quantity * dto.KoliIciAdet;
            entity.KoliToplamAgirligiKg = entity.Quantity * dto.ApproximateWeightKg;
            entity.ListeFiyat = dto.KoliIciAdet > 0 ? dto.Price / dto.KoliIciAdet : 0;
            entity.OneriRafFiyati = dto.OneriRafFiyati ?? 0;
            entity.OneriAksiyonFiyati = dto.OneriAksiyonFiyati ?? 0;
            entity.AksiyonSatisFiyati = dto.AksiyonSatisFiyati;

            decimal toplam = dto.Price * entity.Quantity;
            if (dto.Iskonto1 > 0) toplam *= (100 - dto.Iskonto1) / 100m;
            if (dto.Iskonto2 > 0) toplam *= (100 - dto.Iskonto2) / 100m;
            if (dto.Iskonto3 > 0) toplam *= (100 - dto.Iskonto3) / 100m;
            if (dto.Iskonto4 > 0) toplam *= (100 - dto.Iskonto4) / 100m;

            if (dto.SabitBedelTL > 0)
            {
                toplam -= (decimal)dto.SabitBedelTL;
                if (toplam < 0) toplam = 0;
            }

            decimal sonAdetFiyat = entity.KoliIciToplamAdet > 0 ? toplam / entity.KoliIciToplamAdet : 0;
            if (dto.AdetFarkDonusuTL > 0)
            {
                sonAdetFiyat -= dto.AdetFarkDonusuTL;
                if (sonAdetFiyat < 0) sonAdetFiyat = 0;
            }

            decimal finalToplam = Math.Round(sonAdetFiyat * entity.KoliIciToplamAdet, 2);

            entity.SonAdetFiyati = Math.Round(sonAdetFiyat, 2);
            entity.Total = finalToplam;
            entity.Maliyet = entity.BrutTotal != 0
                ? Math.Round(((entity.BrutTotal - entity.Total) / entity.BrutTotal) * 100, 2)
                : 0;

            _talepFormRepository.Create(entity);

            return entity; //artık kaydedilen formu geri döndürüyoruz
        }

        public List<TalepForm> TGetAllByUserId(int userId)
        {
            return _talepFormRepository.GetAllByUserId(userId);
        }

        public List<TalepForm> TGetAllWithUser()
        {
            return _talepFormRepository.GetAllWithUser();
        }
        
        public TalepForm TGetByIdWithUserAndPoint(int id)
        {
            return _talepFormRepository.GetByIdWithUserAndPoint(id);//gelen mail için:taleplerde bulunan kullanıcı adı ve point ismi lazım
        }

        public TalepForm TGetDetailsForForm(int formId)
        {
            return _talepFormRepository.GetDetailsForForm(formId);
        }
        public void TUpdateItemFields(int TalepFormitemId, int quantity, DateTime validFrom, DateTime validTo)
        {
            _talepFormRepository.UpdateItemFields(TalepFormitemId, quantity, validFrom, validTo);
        }

        public void TUpdateStatus(int formId, TalepDurumu durum, int adminId)
        {
            _talepFormRepository.UpdateStatus(formId, durum, adminId);
        }
    }
}