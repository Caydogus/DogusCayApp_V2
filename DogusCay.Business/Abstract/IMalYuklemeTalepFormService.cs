using DogusCay.DTO.DTOs.ExcelDtos;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.Entity.Entities.MalYuklemeTalep;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Abstract
{
    public interface IMalYuklemeTalepFormService: IGenericService<MalYuklemeTalepForm>
    {
        List<MalYuklemeTalepForm> TGetAllWithUser(); // Bölge Müdürü için
        List<MalYuklemeTalepForm> TGetAllByUserId(int userId); // Kullanıcıya özel
        void TUpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
        MalYuklemeTalepForm TGetDetailsForForm(int id); // Navigation dahil detay

        MalYuklemeTalepForm TCreateMalYuklemeTalepForm(CreateMalYuklemeTalepFormDto dto, int authenticatedUserId);
        public List<ResultMalYuklemeTalepFormDto> TGetAllForIndex();
        public MalYuklemeTalepForm TGetByIdWithUserAndPoint(int id); //gelen mail için:taleplerde bulunan kullanıcı adı ve point ismi lazım
                                                                   
        List<ExportMalYuklemeTalepFormDto> TGetAllForExport(); // Admin için tüm formlar (Excel export) excele artar
        List<ExportMalYuklemeTalepFormDto> TGetListForExportByUserId(int userId); // Kullanıcıya özel formlar (Excel export)
        List<ExportMalYuklemeTalepFormDetailDto> TGetAllDetailsForExport(); // Admin için tüm detaylar (Excel export)
        List<ExportMalYuklemeTalepFormDetailDto> TGetDetailsForExportByUserId(int userId); // Kullanıcıya özel detaylar (Excel export)

    }
}
