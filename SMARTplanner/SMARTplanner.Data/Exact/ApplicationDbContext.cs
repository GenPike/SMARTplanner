using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities;

namespace SMARTplanner.Data.Exact
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, ISmartPlannerContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUserAccess> ProjectUserAccesses { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Report> Reports { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
