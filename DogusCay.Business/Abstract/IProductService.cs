using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        void TShowOnHome(int id);
        void TDontShowOnHome(int id);

        List<Product> TGetAllProductsWithCategories();

        List<Product> TGetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null);
    }
}
