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

        public List<Point> TGetByPointGroupId(int pointGroupId)
        {
           return _pointRepository.GetByPointGroupId(pointGroupId);
        }
    }
}