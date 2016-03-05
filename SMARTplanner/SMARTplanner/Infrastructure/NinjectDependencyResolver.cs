using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using SMARTplanner.Logic.Contracts;
using SMARTplanner.Logic.Exact;

namespace SMARTplanner.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<Data.Contracts.ISmartPlannerContext>().To<Data.Exact.ApplicationDbContext>();
            _kernel.Bind<IProjectService>().To<ProjectService>();
        }
    }
}