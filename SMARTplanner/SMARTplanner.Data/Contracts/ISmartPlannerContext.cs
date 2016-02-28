using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SMARTplanner.Data.Contracts
{
    public interface ISmartPlannerContext : IDisposable
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
