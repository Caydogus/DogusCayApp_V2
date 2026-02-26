using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities.IhaleAnlasma;
using DogusCay.Entity.Entities.Talep;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Concrete
{
    public class IhaleAnlasmaSozlesmeRepository : GenericRepository<IhaleAnlasmaSozlesme>, IIhaleAnlasmaSozlesmeRepository
    {
        private readonly DogusCayContext _context;

        public IhaleAnlasmaSozlesmeRepository(DogusCayContext context) : base(context)
        {
            _context = context;
        }

        // --- İhale Anlaşma Listeleme (Read-Only, linked server tablosu) ---
        public List<IhaleAnlasma> GetAnlasmaListByUserId(int userId)
        {
            return _context.IhaleAnlasmalar
                .Where(x => x.AppUserId == userId)
                .OrderBy(x => x.NoktaAdi)
                .ToList();
        }

        // --- Sözleşme İşlemleri ---
        public IhaleAnlasmaSozlesme GetByNoktaKod(string noktaKod)
        {
            return _context.IhaleAnlasmaSozlesmeler
                .Include(s => s.Dosyalar)
                .FirstOrDefault(s => s.NoktaKod == noktaKod);
        }

        public IhaleAnlasmaSozlesme GetDetailsById(int id)
        {
            return _context.IhaleAnlasmaSozlesmeler
                .Include(s => s.AppUser)
                .Include(s => s.OnaylayanAdmin)
                .Include(s => s.Dosyalar.OrderBy(d => d.SayfaSirasi))
                .FirstOrDefault(s => s.IhaleAnlasmaSozlesmeId == id);
        }

        public List<IhaleAnlasmaSozlesme> GetAllForAdmin()
        {
            return _context.IhaleAnlasmaSozlesmeler
                .Include(s => s.AppUser)
                .Include(s => s.Dosyalar)
                .OrderByDescending(s => s.IhaleAnlasmaSozlesmeId)
                .ToList();
        }

        public List<IhaleAnlasmaSozlesme> GetAllByUserId(int userId)
        {
            return _context.IhaleAnlasmaSozlesmeler
                .Where(s => s.AppUserId == userId)
                .Include(s => s.Dosyalar)
                .OrderByDescending(s => s.IhaleAnlasmaSozlesmeId)
                .ToList();
        }

        public void UpdateStatus(int sozlesmeId, TalepDurumu durum, int adminId)
        {
            var sozlesme = _context.IhaleAnlasmaSozlesmeler.Find(sozlesmeId);
            if (sozlesme != null)
            {
                sozlesme.TalepDurumu = durum;
                sozlesme.OnaylayanAdminId = adminId;
                _context.SaveChanges();
            }
        }
        public List<IhaleAnlasma> GetAllAnlasma()
        {
            return _context.IhaleAnlasmalar.OrderBy(x => x.NoktaAdi).ToList();
        }

        public IhaleAnlasma GetAnlasmaByNoktaKod(string noktaKod)
        {
            return _context.IhaleAnlasmalar.FirstOrDefault(x => x.NoktaKod == noktaKod);
        }
        public IhaleAnlasmaDosya GetDosyaById(int dosyaId)
        {
            return _context.Set<IhaleAnlasmaDosya>()
                .Include(d => d.IhaleAnlasmaSozlesme)
                .FirstOrDefault(d => d.IhaleAnlasmaDosyaId == dosyaId);
        }

        public void DeleteDosya(int dosyaId)
        {
            var dosya = _context.Set<IhaleAnlasmaDosya>().Find(dosyaId);
            if (dosya != null)
            {
                _context.Set<IhaleAnlasmaDosya>().Remove(dosya);
                _context.SaveChanges();
            }
        }
    }
}