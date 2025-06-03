using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DogusCayContext _context;
        public GenericRepository(DogusCayContext context)
        {
            _context = context;
        }
        public DbSet<T> Table { get => _context.Set<T>(); }
        public int Count()
        {
            return Table.Count();
        }
        public void Create(T entity)
        {
            try
            {
                Table.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Db SaveChanges Hatası: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("📛 Inner Exception: " + ex.InnerException.Message);
                throw; // isteğe bağlı: tekrar fırlatmak istersen
            }
        }
        //public void Create(T entity)
        //{
        //    Table.Add(entity);
        //    _context.SaveChanges();
        //}

        public void Delete(int id)
        {
            var entity = Table.Find(id);
            Table.Remove(entity);
            _context.SaveChanges();
        }

        public int FilteredCount(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).Count();
        }

        public T GetByFilter(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).FirstOrDefault();
        }

        public T GetById(int id)
        {
            return Table.Find(id);
        }

        public List<T> GetFilteredList(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).ToList();
        }

        public List<T> GetList()
        {
            return Table.ToList();
        }

        public void Update(T entity)
        {
            Table.Update(entity);
            _context.SaveChanges();
        }
    }
}
