using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGenericRepository.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        DbContext Context { get; }

        IQueryable<TEntity> GetAll();

        Task<TEntity> Get(params object[] keys);

        Task<TEntity> Save(TEntity entity);

        Task Delete(TEntity entityToDelete);
    }
}