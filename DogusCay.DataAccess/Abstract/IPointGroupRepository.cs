using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IPointGroupRepository:IRepository<PointGroup>
    {

        //nokta gruplarını getirirken hangi kanala bağlı oldugunu gormek için. kanal ismini yazmak için
        public List<PointGroup> GetPointGroupsWithKanal();
    }
}
