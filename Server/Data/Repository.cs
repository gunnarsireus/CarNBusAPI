using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Server.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
	    protected readonly DbContext Context;

	    protected Repository(DbContext context)
	    {
		    Context = context;
		}

		public void Add(TEntity entity)
		{
			Context.Add(entity);
		}

	    public void Update(TEntity entity)
	    {
			Context.Update(entity);
	    }

		public void AddRange(IEnumerable<TEntity> entities)
		{
			Context.AddRange(entities);
		}

		public void Remove(TEntity entity)
		{
			Context.Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			Context.RemoveRange(entities);
		}
	}
}
