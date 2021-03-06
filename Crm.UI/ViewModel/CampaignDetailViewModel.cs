using Crm.Model;
using Crm.UI.Data.Repositories;
using Crm.UI.View.Services;
using Crm.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Crm.UI.ViewModel
{
    public class CampaignDetailViewModel : DetailViewModelBase, ICampaignDetailViewModel
    {
        private ICampaignRepository _repository;
        private IMessageDialogService _messgageDialogService;
        private CampaignWrapper _campaign;

        public CampaignWrapper Campaign
        {
            get { return _campaign; }
            private set
            {
                _campaign = value;
                OnPropertyChanged();
            }
        }

        public CampaignDetailViewModel(ICampaignRepository repository,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService) : base(eventAggregator)
        {
            _repository = repository;
            _messgageDialogService = messageDialogService;
        }



        public override async Task LoadAsync(int? id)
        {
            var campaign = id.HasValue ?
                await _repository.GetByIdAsync(id.Value) : CreateNewCampaign();

            InitializeCampaign(campaign);
        }

        private void InitializeCampaign(Campaign campaign)
        {
            Campaign = new CampaignWrapper(campaign);
            Campaign.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _repository.HasChanges();
                }
                if (e.PropertyName == nameof(Campaign.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Campaign.Id == 0)
                campaign.Title = "";
        }

        private Campaign CreateNewCampaign()
        {
            var campaign = new Campaign()
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date,
            };
            _repository.Add(campaign);
            return campaign;
        }

        protected override async void OnSaveExecute()
        {
            await _repository.SaveAsync();
            HasChanges = _repository.HasChanges();
            RaiseDetailSavedEvent(Campaign.Id, Campaign.Title);
        }

        protected override bool OnSaveCanExecute()
        {
            return Campaign != null && !Campaign.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messgageDialogService.ShowOkCancelDialog($"Do you really want to delete {Campaign.Title}", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _repository.Remove(Campaign.Model);
                await _repository.SaveAsync();
                RaiseDetailDeletedEvent(Campaign.Id);
            }
        }
    }
}
