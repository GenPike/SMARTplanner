﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Data.Exact;

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
            _kernel.Bind<ISmartPlannerContext>().To<ApplicationDbContext>();
            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}