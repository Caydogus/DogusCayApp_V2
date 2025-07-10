using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Abstract
{
    public interface IPointService : IGenericService<Point>
    {
        List<Point> TGetByDistributorAndGroup(int distributorId, int groupTypeId);
        List<Point> TGetByKanalId(int KanalId);
        List<Point> TGetListWithIncludes();
        Point TGetDetailsById(int id);
    }
}
