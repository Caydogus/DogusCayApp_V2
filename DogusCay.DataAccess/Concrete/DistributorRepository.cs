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
    public class DistributorRepository : GenericRepository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(DogusCayContext context) : base(context)
        {
        }

        public List<Distributor> GetDistributorsByKanalId(int KanalId)
        {
            return _context.Distributors
                                        .Where(d => d.KanalId == KanalId)
                                        .ToList();
        }

        public List<Distributor> GetDistributorsWithPoints()
        {
            return _context.Distributors
                                       .Include(d => d.Points)
                                       .ToList();
        }

        public List<Distributor> GetListWithAppUser()
        {
            return _context.Distributors
                                      .Include(d => d.AppUser)
                                      .ToList();
        }
    }
}
