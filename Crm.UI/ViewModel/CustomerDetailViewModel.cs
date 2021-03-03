using Crm.Model;
using Crm.UI.Data.Lookups;
using Crm.UI.Data.Repositories;
using Crm.UI.Event;
using Crm.UI.View.Services;
using Crm.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crm.UI.ViewModel
{
    public class CustomerDetailViewModel : DetailViewModelBase, ICustomerDetailViewModel
    {
        private ICustomerRepository _repository;
        private IMessageDialogService _messgageDialogService;
        private ICustomerTypeLookupDataService _customerTypeLookupDataService;
        private CustomerWrapper _customer;
        private CustomerContactWrapper _selectedContact;
        private bool _hasChanges;

        public ICommand AddContactCommand { get; }
        public ICommand RemoveContactCommand { get; }

        public ObservableCollection<LookupItem> CustomerTypes { get; }
        public ObservableCollection<CustomerContactWrapper> Contacts { get; }

        public CustomerWrapper Customer
        {
            get { return _customer; }
            private set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        public CustomerContactWrapper SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveContactCommand).RaiseCanExecuteChanged();
            }
        }

        public CustomerDetailViewModel(ICustomerRepository repository,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService,
          ICustomerTypeLookupDataService customerTypeLookupDataService) : base(eventAggregator)
        {
            _repository = repository;
            _messgageDialogService = messageDialogService;
            _customerTypeLookupDataService = customerTypeLookupDataService;

            AddContactCommand = new DelegateCommand(OnAddContactExecute);
            RemoveContactCommand = new DelegateCommand(OnRemoveContactExecute, OnRemoveContactCanExecute);

            CustomerTypes = new ObservableCollection<LookupItem>();
            Contacts = new ObservableCollection<CustomerContactWrapper>();
        }



        public override async Task LoadAsync(int? customerId)
        {
            var customer = customerId.HasValue ?
                await _repository.GetByIdAsync(customerId.Value) : CreateNewCustomer();

            InitializeCustomer(customer);
            InitializeCustomerContacts(customer.CustomerContacts);
            await LoadCustomerTypeLookup();
        }

        private void InitializeCustomerContacts(ICollection<CustomerContact> customerContacts)
        {
            foreach (var wrapper in Contacts)
            {
                wrapper.PropertyChanged -= CustomerContactWrapper_PropertyChanged;
            }
            Contacts.Clear();
            foreach (var item in customerContacts)
            {
                var wrapper = new CustomerContactWrapper(item);
                Contacts.Add(wrapper);
                wrapper.PropertyChanged += CustomerContactWrapper_PropertyChanged;
            }
        }

        private void CustomerContactWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _repository.HasChanges();
            }
            if (e.PropertyName == nameof(CustomerContactWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeCustomer(Customer customer)
        {
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

        private async Task LoadCustomerTypeLookup()
        {
            CustomerTypes.Clear();
            CustomerTypes.Add(new NullLookupItem { DisplayMember = "[Not Selected]" });

            var lookup = await _customerTypeLookupDataService.GetCustomerTypeLookupAsync();
            foreach (var lookupItem in lookup)
            {
                CustomerTypes.Add(lookupItem);
            }
        }

        private Customer CreateNewCustomer()
        {
            var customer = new Customer();
            _repository.Add(customer);
            return customer;
        }

        protected override async void OnSaveExecute()
        {
            await _repository.SaveAsync();
            HasChanges = _repository.HasChanges();
            RaiseDetailSavedEvent(Customer.Id, $"{Customer.Name} - {Customer.Code}");
        }

        protected override bool OnSaveCanExecute()
        {
            return Customer != null && !Customer.HasErrors && Contacts.All(x => !x.HasErrors) && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messgageDialogService.ShowOkCancelDialog($"Do you really want to delete {Customer.Name}", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _repository.Remove(Customer.Model);
                await _repository.SaveAsync();
                RaiseDetailDeletedEvent(Customer.Id);
            }
        }

        private bool OnRemoveContactCanExecute()
        {
            return SelectedContact != null;
        }

        private void OnRemoveContactExecute()
        {
            SelectedContact.PropertyChanged -= CustomerContactWrapper_PropertyChanged;
            _repository.RemovePhoneNumber(SelectedContact.Model);
            //Customer.Model.CustomerContacts.Remove(SelectedContact.Model);
            Contacts.Remove(SelectedContact);
            SelectedContact = null;
            HasChanges = _repository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddContactExecute()
        {
            var newContact = new CustomerContactWrapper(new CustomerContact());
            newContact.PropertyChanged += CustomerContactWrapper_PropertyChanged;
            Contacts.Add(newContact);
            Customer.Model.CustomerContacts.Add(newContact.Model);
            newContact.Number = "";
        }
    }
}
