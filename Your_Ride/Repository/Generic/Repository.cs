using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;

namespace Your_Ride.Repository.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Context context;
        private readonly DbSet<T> dbSet;

        public Repository(Context _context)
        {
            context = _context;
            dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            List<T> entities = await dbSet.ToListAsync();
            if (entities == null)
            {
                return null;
            }
            return entities;
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
          await  SaveDB();
            return entity;
        }
        public async Task<int> DisposeAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return 0;
            }
            else
            {
                dbSet.Remove(entity);
               await SaveDB();
                return 1;
            }

        }
        public async Task<T> UpdateAsync(T entity)
        {
            dbSet.Update(entity);
           await SaveDB();
            return entity;
        }

        public async Task SaveDB()
        {
            await context.SaveChangesAsync();
        }
    }
}
