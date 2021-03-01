using Crm.Model;
using Crm.UI.Data;
using Crm.UI.Data.Repositories;
using Crm.UI.Event;
using Crm.UI.View.Services;
using Crm.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crm.UI.ViewModel
{
    public class CustomerDetailViewModel : ViewModelBase, ICustomerDetailViewModel
    {
        private ICustomerRepository _repository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messgageDialogService;
        private CustomerWrapper _customer;

        public CustomerDetailViewModel(ICustomerRepository repository,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            _messgageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private async void OnDeleteExecute()
        {
            var result = _messgageDialogService.ShowOkCancelDialog($"Do you really want to delete {Customer.Name}", "Question");
            if(result == MessageDialogResult.Ok)
            {
                _repository.Remove(Customer.Model);
                await _repository.SaveAsync();
                _eventAggregator.GetEvent<AfterCustomerDeletedEvent>().Publish(Customer.Id);
            }
        }

        public async Task LoadAsync(int? customerId)
        {
            var customer = customerId.HasValue ?
                await _repository.GetByIdAsync(customerId.Value) : CreateNewCustomer();

            Customer = new CustomerWrapper(customer);
            Customer.PropertyChanged += (s, e) =>
              {
                  if (!HasChanges)
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

        private Model.Customer CreateNewCustomer()
        {
            var customer = new Customer();
            _repository.Add(customer);
            return customer;
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
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

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
