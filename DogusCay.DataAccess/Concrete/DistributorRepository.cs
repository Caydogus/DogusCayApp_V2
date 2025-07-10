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

        // YENİ İMPLEMENTASYON: Belirli bir AppUserId'ye bağlı distribütörleri getirir.
        public List<Distributor> GetDistributorsByAppUserId(int appUserId)
        {
            // _context, GenericRepository'den gelen protected bir DbContext örneği olmalıdır.
            // AppUser navigasyon özelliğini de dahil etmek isteyebilirsiniz, bu sayede 
            // distribütör ile ilişkilendirilmiş kullanıcı bilgileri de getirilir.
            return _context.Distributors
                           .Include(d => d.AppUser) // İsteğe bağlı: AppUser bilgisini de çekmek için
                           .Where(d => d.AppUserId == appUserId)
                           .ToList();
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