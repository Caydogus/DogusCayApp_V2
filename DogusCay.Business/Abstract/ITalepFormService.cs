using DogusCay.Business.Abstract;
using DogusCay.Entity.Entities.Talep;

public interface ITalepFormService : IGenericService<TalepForm>
{
    List<TalepForm> TGetAllWithUser(); // Admin için tüm talepler
    List<TalepForm> TGetAllByUserId(int userId); // Kullanıcıya özel
    void TUpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
    TalepFormItem TGetItemById(int itemId); // Ürünle birlikte
    void TUpdateItemFields(int TalepFormitemId, int quantity, DateTime? validFrom, DateTime? validTo); // Güvenli alan güncelleme
    TalepForm TGetDetailsForForm(int formId); // Tek talep detay

}
