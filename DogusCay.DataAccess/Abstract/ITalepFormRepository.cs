using DogusCay.Entity.Entities.Talep;
using System.Collections.Generic;

namespace DogusCay.DataAccess.Abstract
{
    public interface ITalepFormRepository : IRepository<TalepForm>
    {
        List<TalepForm> GetAllWithUser(); // Admin için tüm talepler
        List<TalepForm> GetAllByUserId(int userId); // Kullanıcıya özel
        void UpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
        void UpdateItemFields(int TalepFormitemId, int quantity, DateTime validFrom, DateTime validTo); // Güvenli alan güncelleme
        TalepForm GetDetailsForForm(int formId); // Tek talep detay
        TalepForm GetByIdWithUserAndPoint(int id);//gelen mail için:taleplerde bulunan kullanıcı adı ve point ismi lazım

    }
}
