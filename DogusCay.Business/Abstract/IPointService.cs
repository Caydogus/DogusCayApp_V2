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
        List<Point> TGetByPointGroupId(int pointGroupId);
    }
}
