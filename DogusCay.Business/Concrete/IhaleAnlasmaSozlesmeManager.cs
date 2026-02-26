using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DTO.DTOs.IhaleAnlasmaDtos;
using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Concrete
{
    public class IhaleAnlasmaSozlesmeManager : GenericManager<IhaleAnlasmaSozlesme>, IIhaleAnlasmaSozlesmeService
    {
        private readonly IIhaleAnlasmaSozlesmeRepository _repository;

        public IhaleAnlasmaSozlesmeManager(
            IRepository<IhaleAnlasmaSozlesme> genericRepository,
            IIhaleAnlasmaSozlesmeRepository repository)
            : base(genericRepository)
        {
            _repository = repository;
        }

        public List<IhaleAnlasma> TGetAnlasmaListByUserId(int userId)
        {
            return _repository.GetAnlasmaListByUserId(userId);
        }

        public IhaleAnlasmaSozlesme TCreateSozlesme(CreateIhaleAnlasmaSozlesmeDto dto, int userId, List<IhaleAnlasmaDosya> dosyalar)
        {
            // Bu nokta için zaten sözleşme var mı
            var mevcut = _repository.GetByNoktaKod(dto.NoktaKod);
            if (mevcut != null)
                throw new InvalidOperationException("Bu nokta için zaten bir sözleşme yüklenmiş.");

            // Nokta kullanıcıya ait mi kontrol et
            var anlasmaList = _repository.GetAnlasmaListByUserId(userId);
            var anlasma = anlasmaList.FirstOrDefault(a => a.NoktaKod == dto.NoktaKod);
            if (anlasma == null)
                throw new UnauthorizedAccessException("Bu nokta size ait değil.");

            if (dosyalar == null || !dosyalar.Any())
                throw new InvalidOperationException("En az bir dosya yüklenmeli.");

            var sozlesme = new IhaleAnlasmaSozlesme
            {
                NoktaKod = dto.NoktaKod,
                AppUserId = userId,
                IskontoOrani = dto.IskontoOrani,
                CreateDate = DateTime.Now,
                TalepDurumu = TalepDurumu.Bekliyor,
                Note = dto.Note,
                Dosyalar = dosyalar
            };

            base.TCreate(sozlesme);
            return sozlesme;
        }

        public IhaleAnlasmaSozlesme TGetByNoktaKod(string noktaKod)
        {
            return _repository.GetByNoktaKod(noktaKod);
        }

        public IhaleAnlasmaSozlesme TGetDetailsById(int id)
        {
            return _repository.GetDetailsById(id);
        }

        public List<IhaleAnlasmaSozlesme> TGetAllForAdmin()
        {
            return _repository.GetAllForAdmin();
        }

        public List<IhaleAnlasmaSozlesme> TGetAllByUserId(int userId)
        {
            return _repository.GetAllByUserId(userId);
        }

        public void TUpdateStatus(int sozlesmeId, TalepDurumu durum, int adminId)
        {
            var sozlesme = _repository.GetById(sozlesmeId);
            if (sozlesme == null)
                throw new InvalidOperationException($"SozlesmeId: {sozlesmeId} bulunamadı.");

            sozlesme.TalepDurumu = durum;
            sozlesme.OnaylayanAdminId = adminId;
            _repository.Update(sozlesme);
        }

        public List<IhaleAnlasma> TGetAllAnlasma()
        {
            return _repository.GetAllAnlasma();
        }

        public IhaleAnlasma TGetAnlasmaByNoktaKod(string noktaKod)
        {
            return _repository.GetAnlasmaByNoktaKod(noktaKod);
        }
        public IhaleAnlasmaDosya TGetDosyaById(int dosyaId)
        {
            return _repository.GetDosyaById(dosyaId);
        }

        public void TDeleteDosya(int dosyaId)
        {
            _repository.DeleteDosya(dosyaId);
        }
    }
}