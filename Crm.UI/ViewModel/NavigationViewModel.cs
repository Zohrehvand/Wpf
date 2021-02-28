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

        public NavigationViewModel(ICustomerLookupDataService customerLookupService,
          IEventAggregator eventAggregator)
        {
            _customerLookupService = customerLookupService;
            _eventAggregator = eventAggregator;
            Customers = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterCustomerSavedEvent>().Subscribe(AfterCustomerSaved);
        }

        private void AfterCustomerSaved(AfterCustomerSavedEventArgs obj)
        {
            var lookupItem = Customers.Single(l => l.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _customerLookupService.GetCustomerLookupAsync();
            Customers.Clear();
            foreach (var item in lookup)
            {
                Customers.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Customers { get; }

        private NavigationItemViewModel _selectedCustomer;

        public NavigationItemViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                if (_selectedCustomer != null)
                {
                    _eventAggregator.GetEvent<OpenCustomerDetailViewEvent>()
                      .Publish(_selectedCustomer.Id);
                }
            }
        }

    }
}
