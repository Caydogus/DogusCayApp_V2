using System.Linq.Expressions;
using DogusCay.DTOs.ProductDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
       
        List<Product> GetAllProductsWithCategories();
        List<ResultProductDto> GetAllProductsWithCategoryDetails();//kategorileri ve tum alt kategorileride getirsin:09.05.2025

        List<Product> GetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null);
        void ShowOnHome(int id);
        void DontShowOnHome(int id);
        // Yeni eklenenler:
        List<Product> GetProductsBySubCategoryId(int subCategoryId);
        Product GetProductWithCategory(int productId);
        Product? GetProductWithDetails(int productId);
    }
}
