using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetAllWithProducts(); //alt kategorilere tıklayınca tum ürünleri getirir:08.05.2025 eklendi
        void ShowOnHome(int id);
        void DontShowOnHome(int id);
    }
}
