using Crm.UI.Data.Lookups;
using Crm.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ICustomerLookupDataService _customerLookupService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Customers { get; }

        public NavigationViewModel(ICustomerLookupDataService customerLookupService,
          IEventAggregator eventAggregator)
        {
            _customerLookupService = customerLookupService;
            _eventAggregator = eventAggregator;
            Customers = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _customerLookupService.GetCustomerLookupAsync();
            Customers.Clear();
            foreach (var item in lookup)
            {
                Customers.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(CustomerDetailViewModel), _eventAggregator));
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CustomerDetailViewModel):
                    var customer = Customers.SingleOrDefault(x => x.Id == args.Id);
                    if (customer != null) Customers.Remove(customer);
                    break;
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CustomerDetailViewModel):
                    var lookupItem = Customers.SingleOrDefault(l => l.Id == args.Id);
                    if (lookupItem == null)
                    {
                        Customers.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, nameof(CustomerDetailViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = args.DisplayMember;
                    }
                    break;
            }
        }
    }
}
