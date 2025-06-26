using System.Linq.Expressions;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DTOs.ProductDtos;
using DogusCay.Entity.Entities;


namespace DogusCay.Business.Concrete
{
    public class ProductManager : GenericManager<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductManager(IRepository<Product> _repository, IProductRepository ProductRepository) : base(_repository)
        {
            _productRepository = ProductRepository;
        }

        public void TDontShowOnHome(int id)
        {
            _productRepository.DontShowOnHome(id);
        }

        public List<Product> TGetAllProductsWithCategories()
        {
            return _productRepository.GetAllProductsWithCategories();
        }

        public List<Product> TGetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null)
        {
            return _productRepository.GetAllProductsWithCategories(filter);
        }
        //kategorileri ve tum alt kategorileride getirsin:09.05.2025
        public List<ResultProductDto> TGetAllProductsWithCategoryDetails()
        {
            return _productRepository.GetAllProductsWithCategoryDetails();
        }

        public List<Product> TGetProductsBySubCategoryId(int subCategoryId)
        {
            return _productRepository.GetProductsBySubCategoryId((int)subCategoryId);
        }

        public Product TGetProductWithCategory(int productId)
        {
            return _productRepository.GetProductWithCategory(productId);
        }

        public void TShowOnHome(int id)
        {
            _productRepository.ShowOnHome(id);
        }

        public Product? TGetProductDetailsById(int productId)
        {
            return _productRepository.GetProductWithDetails(productId);
        }
    }

}
