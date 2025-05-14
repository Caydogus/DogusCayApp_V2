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
    public class PointGroupRepository : GenericRepository<PointGroup>, IPointGroupRepository
    {
        public PointGroupRepository(DogusCayContext _context) : base(_context)
        {
        }

        // her kanalın altındaki nokta gruplarını getirir
        public List<PointGroup> GetByKanalId(int kanalId)
        {
            return _context.PointGroups
                   .Where(x => x.KanalId == kanalId)
                   .ToList();
        }

        //nokta gruplarını kanalıyla beraber çek.
        public List<PointGroup> GetPointGroupsWithKanal()
        {
            return _context.PointGroups
                           .Include(pg => pg.Kanal)
                           .ToList();
        }

    }
}
