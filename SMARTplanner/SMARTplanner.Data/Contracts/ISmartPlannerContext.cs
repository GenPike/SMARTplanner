using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Data.Contracts
{
    public interface ISmartPlannerContext : IDisposable
    {
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectUserAccess> ProjectUserAccesses { get; set; }
        DbSet<Issue> Issues { get; set; }
        DbSet<WorkItem> WorkItems { get; set; }
        DbSet<Report> Reports { get; set; }

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
