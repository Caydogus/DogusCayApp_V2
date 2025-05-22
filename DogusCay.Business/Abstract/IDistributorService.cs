using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Abstract
{
    public interface IDistributorService:IGenericService<Distributor>
    {
        public List<Distributor> TGetListWithAppUser();//Bölge müdürüne atanmış distributorları görmek için kullanılır.



        //DIST kanalına bağlı distributor’ları getirir
        public List<Distributor> TGetDistributorsByKanalId(int KanalId); // NA ve LC için bu çalışmaz çünkü distributor yok, orası zaten null.



        public List<Distributor> TGetDistributorsWithPoints();//bir distributor’a bağlı noktaları da çekme
    }
}
