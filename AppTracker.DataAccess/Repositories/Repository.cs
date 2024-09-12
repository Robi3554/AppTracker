using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppUsageContext _usageContext;

        protected bool disposed = false;

        public Repository(AppUsageContext context)
        {
            _usageContext = context;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _usageContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<TEntity?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _usageContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> PostAsync(TEntity entity)
        {
            await _usageContext.Set<TEntity>().AddAsync(entity);
            await _usageContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _usageContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;
            await _usageContext.SaveChangesAsync();
            return entity;
        }
    }
}
