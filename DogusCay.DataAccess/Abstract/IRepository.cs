using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DataAccess.Abstract
{
    public interface IRepository<T> where T : class
    {
        List<T> GetList(); // Tüm kayıtları liste olarak getirir
        T GetByFilter(Expression<Func<T, bool>> predicate); // Belirtilen filtreye göre tek kayıt getirir
        T GetById(int id); // Id'ye göre tek kayıt getirir
        void Create(T entity); // Yeni kayıt oluşturur
        void Update(T entity); // Kayıt günceller
        void Delete(int id); // Id'ye göre kayıt siler
        int Count(); // Toplam kayıt sayısını verir
        int FilteredCount(Expression<Func<T, bool>> predicate); // Filtreye uyan kayıt sayısını verir
        List<T> GetFilteredList(Expression<Func<T, bool>> predicate); // Filtreye uyan kayıtları liste olarak getirir
    }
}

