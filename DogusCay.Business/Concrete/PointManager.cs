using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class PointManager : GenericManager<Point>, IPointService
    {
        private readonly IPointRepository _pointRepository;
        public PointManager(IRepository<Point> _repository, IPointRepository pointRepository) : base(_repository)
        {
            _pointRepository = pointRepository;
        }

        public List<Point> TGetByDistributorAndGroup(int distributorId, int groupTypeId)
        {
            return _pointRepository.GetByDistributorAndGroup(distributorId, groupTypeId);
        }

        public List<Point> TGetByKanalId(int KanalId)
        {
            return _pointRepository.GetByKanalId((int)KanalId);
        }

        public Point TGetDetailsById(int id)
        {
           return _pointRepository.GetDetailsById(id);
        }

        public List<Point> TGetListWithIncludes()
        {
            return _pointRepository.GetListWithIncludes();
        }
    }
}