using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Tests.TestHelpers
{
    class TestSmartPlannerContext : ISmartPlannerContext
    {
        public TestSmartPlannerContext()
        {
            Projects = new TestProjectDbSet();
            ProjectUserAccesses = new TestProjectUserAccessDbSet();
            Issues = new TestIssueDbSet();
            WorkItems = new TestWorkItemDbSet();
            Reports = new TestReportDbSet();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectUserAccess> ProjectUserAccesses { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<WorkItem> WorkItems { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<ProjectLog> ProjectsHistory { get; set; }

        public DbSet<IssueLog> IssuesHistory { get; set; }

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose() { }
    }
}
