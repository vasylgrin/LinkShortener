using LinkShortener.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinkShortener.Repository.Repositories
{
    public sealed class MSSQLRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public MSSQLRepository()
        {
            _dbContext = new MSSQLContext();
            _dbSet = _dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.AddAsync(entity);
            _dbContext.SaveChangesAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public TEntity Find(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> query = null;

            foreach (var include in includes)
            {
                query = _dbSet.Include(include);
            }

            return query ?? _dbSet;
        }
        public async Task<TEntity> IncludeAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include)
        {
            return await _dbSet.Include(include).Where(predicate).FirstOrDefaultAsync();
        }

        public ICollection<TEntity> Load()
        {
            return _dbSet.AsNoTracking().ToList();
        }
        public async Task<ICollection<TEntity>> LoadAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        public async Task RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
