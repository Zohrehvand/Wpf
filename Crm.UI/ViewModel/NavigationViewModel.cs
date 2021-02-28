using Crm.UI.Data.Lookups;
using Crm.UI.Event;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ICustomerLookupDataService _customerLookupService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(ICustomerLookupDataService customerLookupService,
          IEventAggregator eventAggregator)
        {
            _customerLookupService = customerLookupService;
            _eventAggregator = eventAggregator;
            Customers = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterCustomerSavedEvent>().Subscribe(AfterCustomerSaved);
            _eventAggregator.GetEvent<AfterCustomerDeletedEvent>().Subscribe(AfterCustomerDeleted);
        }

        private void AfterCustomerDeleted(int customerId)
        {
            var customer = Customers.SingleOrDefault(x => x.Id == customerId);
            if (customer != null) Customers.Remove(customer);
        }

        private void AfterCustomerSaved(AfterCustomerSavedEventArgs obj)
        {
            var lookupItem = Customers.SingleOrDefault(l => l.Id == obj.Id);
            if (lookupItem == null)
            {
                Customers.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _customerLookupService.GetCustomerLookupAsync();
            Customers.Clear();
            foreach (var item in lookup)
            {
                Customers.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Customers { get; }

    }
}
