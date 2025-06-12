using System.Linq.Expressions;
using DearlerPlatform.Core.Core;
using DearlerPlatform.Core.GlobalDTO;
using DearlerPlatform.Domain;

namespace DearlerPlatform.Core.Repository;

public interface IRepository<TEntity>where TEntity: BaseEntity
{
    Task<List<TEntity>> GetListAsync();
    Task<List<TEntity>> GetListAsync(PageWithSortDTO pageWithSortDTO, Expression<Func<TEntity, bool>> WhereCondition);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sort, int skip, int pageSize);
    // IQueryable<TEntity> GetQuaryable(PageWithSortDTO pageWithSortDTO);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    IQueryable<TEntity> GetQueryable();
}
