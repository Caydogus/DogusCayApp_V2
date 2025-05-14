using DogusCay.Entity.Entities.Talep;
using System.Collections.Generic;

namespace DogusCay.DataAccess.Abstract
{
    public interface ITalepFormRepository : IRepository<TalepForm>
    {
        List<TalepForm> GetAllWithUser(); // Admin için tüm talepler
        List<TalepForm> GetAllByUserId(int userId); // Kullanıcıya özel
        void UpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
        TalepFormItem GetItemById(int itemId); // Ürünle birlikte
        void UpdateItemFields(int TalepFormitemId, int quantity, DateTime? validFrom, DateTime? validTo); // Güvenli alan güncelleme
        TalepForm GetDetailsForForm(int formId); // Tek talep detay
    }
}
