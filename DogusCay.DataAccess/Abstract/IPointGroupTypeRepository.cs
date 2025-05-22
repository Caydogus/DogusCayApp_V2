using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IPointGroupTypeRepository:IRepository<PointGroupType>
    {
        List<PointGroupType> GetPointGroupsByDistributorId(int distributorId);
    }
}
