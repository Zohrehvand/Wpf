using Autofac;
using Crm.DataAccess;
using Crm.UI.Data.Lookups;
using Crm.UI.Data.Repositories;
using Crm.UI.View.Services;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<CustomerDetailViewModel>().Keyed<IDetailViewModel>(nameof(CustomerDetailViewModel));
            builder.RegisterType<CampaignDetailViewModel>().Keyed<IDetailViewModel>(nameof(CampaignDetailViewModel));


            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>();

            return builder.Build();
        }
    }
}
