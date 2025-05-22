using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IDistributorRepository : IRepository<Distributor>
    {
        public List<Distributor> GetListWithAppUser();//Bölge müdürüne atanmış distributorları görmek için kullanılır.



        //DIST kanalına bağlı distributor’ları getirir
        public List<Distributor> GetDistributorsByKanalId(int KanalId); // NA ve LC için bu çalışmaz çünkü distributor yok, orası zaten null.
        


        public List<Distributor> GetDistributorsWithPoints();//bir distributor’a bağlı noktaları da çekme
    }
}
