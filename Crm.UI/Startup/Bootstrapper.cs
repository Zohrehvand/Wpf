﻿using Autofac;
using Crm.DataAccess;
using Crm.UI.Data;
using Crm.UI.Data.Lookups;
using Crm.UI.Data.Repositories;
using Crm.UI.ViewModel;
using Prism.Events;

namespace Crm.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<CrmDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<CustomerDetailViewModel>().As<ICustomerDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();

            return builder.Build();
        }
    }
}