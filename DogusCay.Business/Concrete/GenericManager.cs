using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using System.Linq.Expressions;

public class GenericManager<T> : IGenericService<T> where T : class
{
    private readonly IRepository<T> _repository;

    public GenericManager(IRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual List<T> TGetList()
    {
        return _repository.GetList();
    }

    public virtual T TGetByFilter(Expression<Func<T, bool>> predicate)
    {
        return _repository.GetByFilter(predicate);
    }

    public virtual T TGetById(int id)
    {
        return _repository.GetById(id);
    }

    public virtual void TCreate(T entity)
    {
        _repository.Create(entity);
    }

    public virtual void TUpdate(T entity)
    {
        _repository.Update(entity);
    }

    public virtual void TDelete(int id)
    {
        _repository.Delete(id);
    }

    public virtual int TCount()
    {
        return _repository.Count();
    }

    public virtual int TFilteredCount(Expression<Func<T, bool>> predicate)
    {
        return _repository.FilteredCount(predicate);
    }

    public virtual List<T> TGetFilteredList(Expression<Func<T, bool>> predicate)
    {
        return _repository.GetFilteredList(predicate);
    }
}
