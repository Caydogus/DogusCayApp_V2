using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<Point> GetByDistributorAndGroup(int distributorId, int groupTypeId)
        {
            return _context.Points
            .Where(p => p.DistributorId == distributorId && p.PointGroupTypeId == groupTypeId)
            .ToList();
        }

        public List<Point> GetByKanalId(int KanalId)
        {
            return _context.Points
             .Where(p => p.KanalId == KanalId && p.DistributorId == null)
             .ToList();
        }

        public List<Point> GetListWithIncludes()
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
