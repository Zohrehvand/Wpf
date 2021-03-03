using Crm.UI.Event;
using Crm.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crm.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Func<ICustomerDetailViewModel> _customerDetailViewModelCreator;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IDetailViewModel _detailViewModel;

        public ICommand CreateNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(INavigationViewModel navigationViewModel,
          Func<ICustomerDetailViewModel> customerDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            NavigationViewModel = navigationViewModel;
            _customerDetailViewModelCreator = customerDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
             .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel) return;
            }
            switch (args.ViewModelName)
            {
                case nameof(CustomerDetailViewModel):
                    DetailViewModel = _customerDetailViewModelCreator();
                    break;
            }
            DetailViewModel = _customerDetailViewModelCreator();
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
