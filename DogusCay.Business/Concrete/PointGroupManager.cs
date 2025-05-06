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
    public class PointGroupManager : GenericManager<PointGroup>, IPointGroupService
    {
        private readonly IPointGroupService _pointGroupService;

        public PointGroupManager(IRepository<PointGroup> _repository) : base(_repository)
        {
        }
    }
}