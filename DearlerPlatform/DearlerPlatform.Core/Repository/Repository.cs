using System.Linq.Expressions;
using DearlerPlatform.Core.Core;
using DearlerPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using DearlerPlatform.Core.GlobalDTO;

namespace DearlerPlatform.Core.Repository;

public class Repository<TEntity>: IRepository<TEntity> where TEntity: BaseEntity
{
    private readonly DealerPlatformContext _context;
    public Repository(DealerPlatformContext context)
    {
        _context = context;
    }
    public async Task<List<TEntity>> GetListAsync()
    {
        return await GetListAsync(new PageWithSortDTO{
            Sort = "Id",
            PageIndex = 1,
            PageSize = 30
            }, x => true);
    }
    public async Task<List<TEntity>> GetListAsync(PageWithSortDTO pageWithSortDTO, Expression<Func<TEntity, bool>> WhereCondition)
    {
        int skipNum = (pageWithSortDTO.PageIndex -1) * pageWithSortDTO.PageSize;
        if (StringWhiteList.AllowedSortFields.Any(s => s.ToUpper() == pageWithSortDTO.Sort.ToUpper()))
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet
                .Where(WhereCondition)
                .OrderBy($"{pageWithSortDTO.Sort} {pageWithSortDTO.OrderType.ToString().ToUpper()}")
                .Skip(skipNum)
                .Take(pageWithSortDTO.PageSize) 
                .ToListAsync();
        }
        else
        {
            return new List<TEntity>();
        }
    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = _context.Set<TEntity>();
        return await dbSet.Where(predicate).ToListAsync();
    }
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sort, int skip, int pageSize)
    {
        if (StringWhiteList.AllowedSortFields.Contains(sort))
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet
                .Where(predicate)
                .OrderBy($"{sort} ASC")
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
        else
        {
            return new List<TEntity>();
        }
    }
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var dbSet = _context.Set<TEntity>();
        return await dbSet.FirstOrDefaultAsync(predicate);
    }
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var dbSet = _context.Set<TEntity>();
        var res = (await dbSet.AddAsync(entity)).Entity;
        await _context.SaveChangesAsync();
        return res;
    }
    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        var dbSet = _context.Set<TEntity>();
        var res = dbSet.Remove(entity).Entity;
        await _context.SaveChangesAsync();
        return res;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbSet = _context.Set<TEntity>();
        var res = dbSet.Update(entity).Entity;
        await _context.SaveChangesAsync();
        return res;
    }

    public  IQueryable<TEntity> GetQueryable() 
    {
        return _context.Set<TEntity>();
    }
}
