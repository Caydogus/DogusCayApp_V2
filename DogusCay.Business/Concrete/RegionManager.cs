using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class RegionManager:GenericManager<Region>,IRegionService
    {
        private readonly IRegionService _regionService;

        public RegionManager(IRepository<Region> _repository) : base(_repository)
        {
        }
    }
}
