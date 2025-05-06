using System.Linq.Expressions;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities;


namespace DogusCay.Business.Concrete
{
    public class ProductManager : GenericManager<Product>, IProductService
    {
        private readonly IProductRepository _ProductRepository;
        public ProductManager(IRepository<Product> _repository, IProductRepository ProductRepository) : base(_repository)
        {
            _ProductRepository = ProductRepository;
        }

        public void TDontShowOnHome(int id)
        {
            _ProductRepository.DontShowOnHome(id);
        }

        public List<Product> TGetAllProductsWithCategories()
        {
            return _ProductRepository.GetAllProductsWithCategories();
        }

        public List<Product> TGetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null)
        {
            return _ProductRepository.GetAllProductsWithCategories(filter);
        }

        public void TShowOnHome(int id)
        {
            _ProductRepository.ShowOnHome(id);
        }
    }

}
