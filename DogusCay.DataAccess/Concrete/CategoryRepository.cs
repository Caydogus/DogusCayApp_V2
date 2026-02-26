using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DogusCayContext _context) : base(_context)
        {
        }

        public void DontShowOnHome(int id)
        {
            var value = _context.Categories.Find(id);
            value.IsShown = false;
            _context.SaveChanges();
        }

        //alt kategorilere tıklayınca tum ürünleri getirir:08.05.2025 eklendi
        //public List<Category> GetAllWithProducts()
        //{
        //        return _context.Categories
        //                                .Include(c => c.Products)
        //                                .Include(c => c.SubCategories)
        //                                .ThenInclude(sc => sc.Products)
        //                                .Include(c => c.SubCategories)
        //                                .ThenInclude(sc => sc.SubCategories)
        //                                .ThenInclude(ssc => ssc.Products).ToList();
        //}
        public List<Category> GetAllWithProducts()
        {
            var categories = _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Products)
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.SubCategories)
                        .ThenInclude(ssc => ssc.Products)
                .ToList();

            // Manuel filtre
            foreach (var cat in categories)
            {
                cat.Products = cat.Products.Where(p => p.IsShown).ToList();
                foreach (var sub in cat.SubCategories)
                {
                    sub.Products = sub.Products.Where(p => p.IsShown).ToList();
                    foreach (var subsub in sub.SubCategories)
                        subsub.Products = subsub.Products.Where(p => p.IsShown).ToList();
                }
            }

            return categories;
        }
        public void ShowOnHome(int id)
        {
            var value = _context.Categories.Find(id);
            value.IsShown = true;
            _context.SaveChanges();
        }
    }
}
