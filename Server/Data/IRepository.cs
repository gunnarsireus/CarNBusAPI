using System.Collections.Generic;

namespace Server.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
	    void Add(TEntity entity);
	    void Update(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);
	    void Remove(TEntity entity);
	    void RemoveRange(IEnumerable<TEntity> entities);
	}
}
