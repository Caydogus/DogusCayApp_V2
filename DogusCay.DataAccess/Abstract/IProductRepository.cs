using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
       
        List<Product> GetAllProductsWithCategories();
        List<Product> GetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null);
        void ShowOnHome(int id);
        void DontShowOnHome(int id);

    }
}
