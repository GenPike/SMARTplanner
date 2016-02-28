using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;

namespace SMARTplanner.Data.Exact
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ISmartPlannerContext _context;

        public UnitOfWork(ISmartPlannerContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
