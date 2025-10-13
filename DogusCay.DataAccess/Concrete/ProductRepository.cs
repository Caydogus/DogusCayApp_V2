using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.DTOs.ProductDtos;
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
            return _context.Products
                .Include(x => x.Category)
                .Include(x => x.UnitType)   
                .ToList();
        }

        public List<Product> GetAllProductsWithCategories(Expression<Func<Product, bool>> filter = null)
        {
            IQueryable<Product> values = _context.Products
                .Include(x => x.Category).ThenInclude(c => c.ParentCategory) 
                .Include(x => x.UnitType)  
                .AsQueryable();

            if (filter != null)
            {
                values = values.Where(filter);
            }

            return values.ToList();
        }

        //kategorileri ve tum alt kategorileride getirsin:09.05.2025

        public List<ResultProductDto> GetAllProductsWithCategoryDetails()
        {
            return _context.Products
                    .Include(p => p.Category)
                    .ThenInclude(c => c.ParentCategory)
                    .Include(p => p.UnitType)
                    .Select(p => new ResultProductDto
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                ErpCode = p.ErpCode,
                                CategoryId = p.CategoryId,
                                CategoryName = p.Category.CategoryName,
                                ParentCategoryName = p.Category.ParentCategory != null
                                                        ? p.Category.ParentCategory.CategoryName
                                                        : null,
                                UnitTypeId = p.UnitTypeId,
                                UnitTypeName = p.UnitType.UnitTypeName,
                                ApproximateWeightKg = p.ApproximateWeightKg,
                                IsShown = p.IsShown,
                                Price = p.Price,
                            }).ToList();
        }

      
        public void ShowOnHome(int id)
        {
            var value = _context.Products.Find(id);
            value.IsShown = true;
            _context.SaveChanges();
        }
        public List<Product> GetProductsBySubCategoryId(int subCategoryId)
        {
            return _context.Products
                                   .Include(p => p.Category)
                                   .Where(p => p.CategoryId == subCategoryId)
                                   .ToList();
        }

        public Product GetProductWithCategory(int productId)
        {
            return _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefault(p => p.ProductId == productId);
        }

        public Product? GetProductWithDetails(int productId)
        {
            return _context.Products
                .Include(p => p.Category) // Ürünün kendi kategorisini dahil et
                    .ThenInclude(c => c.ParentCategory) // Kategorinin üst kategorisini dahil et
                        .ThenInclude(pc => pc.ParentCategory) // Üst kategorinin üst kategorisini dahil et
                .Include(p => p.UnitType) // Ürünün birim tipini dahil et
                .FirstOrDefault(p => p.ProductId == productId); // Belirtilen ID'ye göre ürünü bul (Senkron)
        }

        //toplu ürün bilgilerini almak için (performanslı)
        public List<Product> GetMultipleProductsInfo(List<int> productIds)
        {
            if (productIds == null || productIds.Count == 0)
                return new List<Product>();

            return _context.Products
                .Include(p => p.Category)
                    .ThenInclude(c => c.ParentCategory)
                .Include(p => p.UnitType)
                .Where(p => productIds.Contains(p.ProductId))
                .ToList();
        }
    }

}   
