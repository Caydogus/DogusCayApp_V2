using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IPointRepository : IRepository<Point>
    {

        List<Point> GetByDistributorAndGroup(int distributorId, int groupTypeId);//Bu metot, özellikle DIST kanalına ait noktaları getirir.
        List<Point> GetByKanalId(int KanalId);//Bu metot, NA veya LC kanalına ait noktaları getirir.

        List<Point> GetListWithIncludes();// ındexte ıdlere gore isimleri getiri UI kısmında
                                         
        Point GetDetailsById(int id); // Bir noktanın detaylarını (ilişkili tablolarla birlikte) ID'ye göre getirir.
    }
}
