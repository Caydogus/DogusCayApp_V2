using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Abstract
{
    public interface IRegionService:IGenericService<Region>
    {
        //bu metot bolgeleri getirirken bölge mudurlerinide getirir
        List<Region> TGetRegionsWithManagers();

    }
}
