using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Server.DataRead
{
    public interface IRepositoryRead<TEntity> where TEntity : class
    {
	    TEntity Get(Guid id);
	    IEnumerable<TEntity> GetAll();
	    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
	}
}
