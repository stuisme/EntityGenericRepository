using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using EntityGenericRepository.Abstractions;
using EntityGenericRepository.Extensions;

namespace EntityGenericRepository.EntityFramework
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class 
    {
        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; private set; }

        public DbSet<TEntity> EntitySet()
        {
            return Context.Set<TEntity>();
        } 

        public virtual IQueryable<TEntity> GetAll()
        {
            return EntitySet();
        }

        public virtual async Task<TEntity> Get(params object[] keys)
        {
            if (keys == null) 
                throw new ArgumentNullException("keys");

            return await EntitySet().FindAsync(keys);
        }

        public virtual async Task<TEntity> Save(TEntity entity)
        {
            if (entity == null) 
                throw new ArgumentNullException("entity");

            var entry = AttachIfNeeded(entity);

            if (KeysAreDefault(entry))
            {
                EntitySet().Add(entity);
            }
            else
            {
                entry.State = EntityState.Modified;
            }

            await Context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) 
                throw new ArgumentNullException("entityToDelete");

            AttachIfNeeded(entityToDelete);

            EntitySet().Remove(entityToDelete);

            await Context.SaveChangesAsync();
        }

        private DbEntityEntry<TEntity> AttachIfNeeded(TEntity entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                EntitySet().Attach(entity);
            }
            return entry;
        }

        private bool KeysAreDefault(DbEntityEntry<TEntity> entry)
        {
            // get underlining object state
            var objectState = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);

            // If any key is not set, consider this object default keys
            return objectState.EntityKey.EntityKeyValues
                .Any(key => key.Value == key.Value.GetType().GetDefaultValue());            
        }

        
    }
}
