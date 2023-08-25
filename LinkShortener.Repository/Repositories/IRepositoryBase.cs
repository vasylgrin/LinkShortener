using System.Linq.Expressions;

namespace LinkShortener.Repository.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        public void Add(TEntity entity);
        public ICollection<TEntity> Load();
        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);
        public void Update(TEntity entity);
        public void Remove(TEntity entity);
        public TEntity Find(int id);
        public TEntity Find(Expression<Func<TEntity, bool>> predicate);



        public Task AddAsync(TEntity entity);
        public Task<ICollection<TEntity>> LoadAsync();
        public Task<TEntity> IncludeAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include);
        public Task UpdateAsync(TEntity entity);
        public Task RemoveAsync(TEntity entity);
        public Task<TEntity> FindAsync(int id);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
