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
    public class PointGroupManager : GenericManager<PointGroup>, IPointGroupService
    {
        private readonly IPointGroupRepository _pointGroupRepository;

        public PointGroupManager(IRepository<PointGroup> _repository, IPointGroupRepository pointGroupRepository) : base(_repository)
        {
            _pointGroupRepository = pointGroupRepository;
        }
        public List<PointGroup> TGetPointGroupsWithKanal()
        {
           return _pointGroupRepository.GetPointGroupsWithKanal();
        }
    }
}