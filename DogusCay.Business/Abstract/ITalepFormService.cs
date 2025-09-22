
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.ExcelDtos;
using DogusCay.DTO.DTOs.TalepFormDtos;
using DogusCay.Entity.Entities.Talep;

public interface ITalepFormService : IGenericService<TalepForm>
{
    List<TalepForm> TGetAllWithUser(); // Admin için tüm talepler
    List<TalepForm> TGetAllByUserId(int userId); // Kullanıcıya özel
    void TUpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
    void TUpdateItemFields(int TalepFormitemId, int quantity, DateTime validFrom, DateTime validTo); // Güvenli alan güncelleme
    TalepForm TGetDetailsForForm(int formId); // Tek talep detay
    public TalepForm CreateTalepFormWithCalculations(CreateTalepFormDto dto, int appUserId);
    TalepForm TGetByIdWithUserAndPoint(int id);//gelen mail için:taleplerde bulunan kullanıcı adı ve point ismi lazım

    List<ExportTalepFormDto> TGetAllForExport();//admin için tüm talepler excele aktar
    List<ExportTalepFormDto> TGetListForExportByUserId(int userId);//kullanıcıya özel talepler excele aktar
}
