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
    public class PointGroupTypeManager : GenericManager<PointGroupType>, IPointGroupTypeService
    {
        private readonly IPointGroupTypeRepository _pointGroupTypeRepository;

        public PointGroupTypeManager(IRepository<PointGroupType> _repository, IPointGroupTypeRepository pointGroupTypeRepository) : base(_repository)
        {
            _pointGroupTypeRepository = pointGroupTypeRepository;
        }

        public List<PointGroupType> TGetPointGroupsByDistributorId(int distributorId)
        {
            return _pointGroupTypeRepository.GetPointGroupsByDistributorId(distributorId);
        }
    }
}