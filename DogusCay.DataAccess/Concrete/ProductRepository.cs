using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using static DogusCay.DataAccess.Concrete.ProductRepository;

namespace DogusCay.DataAccess.Concrete
{
        public class ProductRepository : GenericRepository<Product>, IProductRepository
        {
            public ProductRepository(DogusCayContext context) : base(context)
            {
            }
            public void DontShowOnHome(int id)
            {
                var value = _context.Products.Find(id);
                value.IsShown = false;
                _context.SaveChanges();
            }

            public List<Product> GetAllProductsWithCategories()
            {
                return _context.Products.Include(x => x.Category).ToList();
            }

            public List<Product> GetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null)
            {
                IQueryable<Product> values = _context.Products.Include(x => x.Category).AsQueryable();
                if (filter != null)
                {
                    values = values.Where(filter);
                }

                return values.ToList();
            }

            public void ShowOnHome(int id)
            {
                var value = _context.Products.Find(id);
                value.IsShown = true;
                _context.SaveChanges();
            }
        }
    
}
