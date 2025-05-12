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
        private readonly IRegionRepository _regionRepository;

        public RegionManager(IRepository<Region> _repository, IRegionRepository regionRepository) : base(_repository)
        {
            _regionRepository = regionRepository;
        }
        //bu metot bolgeleri getirirken bölge mudurlerinide getirir

        public List<Region> TGetRegionsWithManagers()
        {
            return _regionRepository.GetRegionsWithManagers();
        }

    }
}
