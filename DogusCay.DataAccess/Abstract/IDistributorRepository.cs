using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions; // Expression için eklendi
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IDistributorRepository : IRepository<Distributor>
    {
        // Bölge müdürüne atanmış distribütörleri görmek için kullanılır.
        public List<Distributor> GetListWithAppUser();

        // DIST kanalına bağlı distribütörleri getirir.
        // NA ve LC için bu çalışmaz çünkü distribütör yok, orası zaten null.
        public List<Distributor> GetDistributorsByKanalId(int KanalId);

        // Bir distribütöre bağlı noktaları da çekme
        public List<Distributor> GetDistributorsWithPoints();

        // YENİ METOT: Belirli bir AppUserId'ye bağlı distribütörleri getirir.
        // Eğer AppUser, Distributor ile birebir veya bire çok ilişkiliyse ve AppUserId Distributor entity'sinde bulunuyorsa bu yeterlidir.
        // Not: Eğer IRepository<Distributor> içinde zaten GetList(Expression<Func<T, bool>> filter) gibi bir metot varsa,
        // bu metodu doğrudan kullanabilirsiniz ve buraya özel bir metot eklemenize gerek kalmayabilir.
        public List<Distributor> GetDistributorsByAppUserId(int appUserId);
    }
}