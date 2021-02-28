using Crm.UI.Event;
using Crm.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,
          Func<ICustomerDetailViewModel> customerDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            NavigationViewModel = navigationViewModel;
            _customerDetailViewModelCreator = customerDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenCustomerDetailViewEvent>()
             .Subscribe(OnOpenCustomerDetailView);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenCustomerDetailView(int customerId)
        {
            if (CustomerDetailViewModel != null && CustomerDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel) return;
            }
            CustomerDetailViewModel = _customerDetailViewModelCreator();
            await CustomerDetailViewModel.LoadAsync(customerId);
        }

        public INavigationViewModel NavigationViewModel { get; }
        //public ICustomerDetailViewModel CustomerDetailViewModel { get; set; }

        private ICustomerDetailViewModel _customerDetailViewModel;

        public ICustomerDetailViewModel CustomerDetailViewModel
        {
            get { return _customerDetailViewModel; }
            set
            {
                _customerDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private Func<ICustomerDetailViewModel> _customerDetailViewModelCreator;

        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
    }
}
