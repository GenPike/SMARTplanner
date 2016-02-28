using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using SMARTplanner.Data.Contracts;

namespace SMARTplanner.Data.Exact
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, ISmartPlannerContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
