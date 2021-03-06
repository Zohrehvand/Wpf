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
        private ICampaignLookupDataService _campaignLookupService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Customers { get; }
        public ObservableCollection<NavigationItemViewModel> Campaigns { get; }

        public NavigationViewModel(ICustomerLookupDataService customerLookupService,
          IEventAggregator eventAggregator,
          ICampaignLookupDataService campaignLookupService)
        {
            _customerLookupService = customerLookupService;
            _campaignLookupService = campaignLookupService;
            _eventAggregator = eventAggregator;
            Customers = new ObservableCollection<NavigationItemViewModel>();
            Campaigns = new ObservableCollection<NavigationItemViewModel>();
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

            lookup = await _campaignLookupService.GetMeetingLookupAsync();
            Campaigns.Clear();
            foreach (var item in lookup)
            {
                 Campaigns.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(CampaignDetailViewModel), _eventAggregator));
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CustomerDetailViewModel):
                    AfterDetailDeleted(Customers, args);
                    break;
                case nameof(CampaignDetailViewModel):
                    AfterDetailDeleted(Campaigns, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(x => x.Id == args.Id);
            if (item != null) items.Remove(item);
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(CustomerDetailViewModel):
                    AfterDetailSaved(Customers, args);
                    break;
                case nameof(CampaignDetailViewModel):
                    AfterDetailSaved(Campaigns, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, 
                   args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}
