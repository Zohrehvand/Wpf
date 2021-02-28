using Crm.UI.Data;
using Crm.UI.Data.Repositories;
using Crm.UI.Event;
using Crm.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crm.UI.ViewModel
{
    public class CustomerDetailViewModel : ViewModelBase, ICustomerDetailViewModel
    {
        private ICustomerRepository _repository;
        private IEventAggregator _eventAggregator;
        private CustomerWrapper _customer;

        public CustomerDetailViewModel(ICustomerRepository repository,
          IEventAggregator eventAggregator)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int customerId)
        {
            var customer = await _repository.GetByIdAsync(customerId);

            Customer = new CustomerWrapper(customer);
            Customer.PropertyChanged += (s, e) =>
              {
                  if(!HasChanges)
                  {
                      HasChanges = _repository.HasChanges();
                  }
                  if (e.PropertyName == nameof(Customer.HasErrors))
                  {
                      ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                  }
              };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public CustomerWrapper Customer
        {
            get { return _customer; }
            private set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if(_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _repository.SaveAsync();
            HasChanges = _repository.HasChanges();
            _eventAggregator.GetEvent<AfterCustomerSavedEvent>().Publish(
              new AfterCustomerSavedEventArgs
              {
                  Id = Customer.Id,
                  DisplayMember = $"{Customer.Name} - {Customer.Code}"
              });
        }

        private bool OnSaveCanExecute()
        {
            return Customer != null && !Customer.HasErrors && HasChanges;
        }
    }
}
