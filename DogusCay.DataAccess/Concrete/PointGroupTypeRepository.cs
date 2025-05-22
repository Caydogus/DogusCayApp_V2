using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.DTO.DTOs.PointGrupDtos;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Concrete
{
    public class PointGroupTypeRepository : GenericRepository<PointGroupType>, IPointGroupTypeRepository
    {
        public PointGroupTypeRepository(DogusCayContext _context) : base(_context)
        {
        }

        public List<PointGroupType> GetPointGroupsByDistributorId(int distributorId)
        {
            return _context.Points
           .Where(p => p.DistributorId == distributorId && p.PointGroupTypeId != null)
           .Include(p => p.PointGroupType)
           .Select(p => p.PointGroupType)
           .Distinct()
           .ToList();
        }
    }
}
