using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DigitalStore.Repos.Interfaces
{
    public interface IRepo<T>
    {
        int Add(T entity);
        int Add(IList<T> entities);
        int Update(T entity);
        int Delete(int id, byte[] timeStamp);
        int Delete(T entity);
        T GetOne(int? id);
        List<T> GetSome(Expression<Func<T, bool>> where);
        List<T> GetAll();
        List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending);
        List<T> ExecuteQuery(FormattableString sql);
        List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);
    }
}
