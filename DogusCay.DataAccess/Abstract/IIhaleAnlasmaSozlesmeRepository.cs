using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.Talep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DataAccess.Abstract
{
    public interface IIhaleAnlasmaSozlesmeRepository : IRepository<IhaleAnlasmaSozlesme>
    {
        List<IhaleAnlasma> GetAnlasmaListByUserId(int userId);
        IhaleAnlasmaSozlesme GetByNoktaKod(string noktaKod);
        IhaleAnlasmaSozlesme GetDetailsById(int id);
        List<IhaleAnlasmaSozlesme> GetAllForAdmin();
        List<IhaleAnlasmaSozlesme> GetAllByUserId(int userId);
        void UpdateStatus(int sozlesmeId, TalepDurumu durum, int adminId);
        List<IhaleAnlasma> GetAllAnlasma();                    // Admin: tüm satırlar
        IhaleAnlasma GetAnlasmaByNoktaKod(string noktaKod);    // Detay için
        IhaleAnlasmaDosya GetDosyaById(int dosyaId);
        void DeleteDosya(int dosyaId);
    }
}