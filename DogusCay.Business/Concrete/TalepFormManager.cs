using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Concrete
{
    // TalepForm ile ilgili iş mantığını yöneten servis katmanı
    public class TalepFormManager : GenericManager<TalepForm>, ITalepFormService
    {
        private readonly ITalepFormRepository _talepFormRepository;

        public TalepFormManager(IRepository<TalepForm> repository, ITalepFormRepository talepFormRepository)
            : base(repository)
        {
            _talepFormRepository = talepFormRepository;
        }

        public List<TalepForm> TGetAllByUserId(int userId)
        {
            return _talepFormRepository.GetAllByUserId(userId);
        }

        public List<TalepForm> TGetAllWithUser()
        {
            return _talepFormRepository.GetAllWithUser();
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