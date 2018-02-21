using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Server.DataRead
{
    public class RepositoryRead<TEntity> : IRepositoryRead<TEntity> where TEntity : class
    {
	    protected readonly DbContext Context;

	    protected RepositoryRead(DbContext context)
	    {
		    Context = context;
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			return Context.Set<TEntity>().Where(predicate);
		}

         public TEntity Get(Guid id)
	    {
			return Context.Set<TEntity>().Find(id);
	    }
	    public IEnumerable<TEntity> GetAll()
	    {
		    return Context.Set<TEntity>().ToList();
	    }
	}
}
