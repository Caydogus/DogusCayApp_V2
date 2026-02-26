using DogusCay.DTO.DTOs.IhaleAnlasmaDtos;
using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Abstract
{
    public interface IIhaleAnlasmaSozlesmeService : IGenericService<IhaleAnlasmaSozlesme>
    {
        List<IhaleAnlasma> TGetAnlasmaListByUserId(int userId);
        IhaleAnlasmaSozlesme TCreateSozlesme(CreateIhaleAnlasmaSozlesmeDto dto, int userId, List<IhaleAnlasmaDosya> dosyalar);
        IhaleAnlasmaSozlesme TGetByNoktaKod(string noktaKod);
        IhaleAnlasmaSozlesme TGetDetailsById(int id);
        List<IhaleAnlasmaSozlesme> TGetAllForAdmin();
        List<IhaleAnlasmaSozlesme> TGetAllByUserId(int userId);
        void TUpdateStatus(int sozlesmeId, TalepDurumu durum, int adminId);
        List<IhaleAnlasma> TGetAllAnlasma();
        IhaleAnlasma TGetAnlasmaByNoktaKod(string noktaKod);
        IhaleAnlasmaDosya TGetDosyaById(int dosyaId);
        void TDeleteDosya(int dosyaId);
    }
}