using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Concrete
{
    public class PointRepository : GenericRepository<PointGroup>, IPointRepository
    {
        public PointRepository(DogusCayContext context) : base(context)
        {
        }

        public List<Point> GetByPointGroupId(int pointGroupId)
        {
            return _context.Points
                                 .Where(p => p.PointGroupId == pointGroupId)
                                 .ToList();
        }
    }
}
