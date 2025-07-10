using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Concrete
{
    public class PointRepository : GenericRepository<Point>, IPointRepository
    {
        public PointRepository(DogusCayContext context) : base(context)
        {
        }
        public List<Point> GetByDistributorAndGroup(int distributorId, int groupTypeId)// DIST kanalına ait noktaları getirir.
        {
            return _context.Points
            .Where(p => p.DistributorId == distributorId && p.PointGroupTypeId == groupTypeId)
            .ToList();
        }

        public List<Point> GetByKanalId(int KanalId)//Bu metot, NA veya LC kanalına ait noktaları getirir.
        {
            return _context.Points
          .Where(p => p.KanalId == KanalId)
          .ToList();
        }

        public Point GetDetailsById(int id)// Bir noktanın detaylarını (ilişkili tablolarla birlikte) ID'ye göre getirir.
        {
            return _context.Points
                           .Include(p => p.Kanal) // Kanal bilgilerini dahil et
                           .Include(p => p.Distributor) // Distribütör bilgilerini dahil et
                           .Include(p => p.PointGroupType) // Nokta Grup Tipi bilgilerini dahil et
                           .Include(p => p.AppUser) // Kullanıcı bilgilerini dahil et
                           .FirstOrDefault(p => p.PointId == id); // ID'ye göre ilkini veya null'ı getir
        }
        public List<Point> GetListWithIncludes()// ındexte ıdlere gore isimleri getiri UI kısmında
        {
            return _context.Points
                                .Include(p => p.Kanal)
                                .Include(p=>p.Distributor)
                                .Include(p=>p.AppUser)
                                .Include(p=>p.PointGroupType)
                                .ToList();
        }
    }
}
