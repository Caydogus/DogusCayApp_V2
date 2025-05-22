using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Concrete;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class DistributorManager : GenericManager<Distributor>, IDistributorService
    {
        private readonly IDistributorRepository _distributorRepository;
        public DistributorManager(IRepository<Distributor> _repository, IDistributorRepository distributorRepository) : base(_repository)
        {
            _distributorRepository = distributorRepository;
        }

        public List<Distributor> TGetDistributorsByKanalId(int KanalId)
        {
            return _distributorRepository.GetDistributorsByKanalId(KanalId);
        }

        public List<Distributor> TGetDistributorsWithPoints()
        {
           return _distributorRepository.GetDistributorsWithPoints();
        }

        public List<Distributor> TGetListWithAppUser()
        {
            return _distributorRepository.GetListWithAppUser();
        }
    }
}
