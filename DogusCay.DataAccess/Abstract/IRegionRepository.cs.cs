using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IRegionRepository : IRepository<Region>
    {
        List<Region> GetRegionsWithManagers();        //bu metot bolgeleri getirirken bölge mudurlerinide getirir

    }
}
