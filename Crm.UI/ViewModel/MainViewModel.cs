using Crm.UI.Event;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,
          Func<ICustomerDetailViewModel> customerDetailViewModelCreator, IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            _customerDetailViewModelCreator = customerDetailViewModelCreator;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<OpenCustomerDetailViewEvent>()
             .Subscribe(OnOpenCustomerDetailView);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenCustomerDetailView(int customerId)
        {
            CustomerDetailViewModel = _customerDetailViewModelCreator();
            await CustomerDetailViewModel.LoadAsync(customerId);
        }

        public INavigationViewModel NavigationViewModel { get; }
        //public ICustomerDetailViewModel CustomerDetailViewModel { get; set; }

        private ICustomerDetailViewModel _customerDetailViewModel;

        public ICustomerDetailViewModel CustomerDetailViewModel
        {
            get { return _customerDetailViewModel; }
            set { _customerDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private Func<ICustomerDetailViewModel> _customerDetailViewModelCreator;

        private IEventAggregator _eventAggregator;
    }
}
