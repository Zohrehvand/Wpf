using Crm.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace Crm.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public ICommand OpenCustomerDetailViewCommand { get; }
        public int Id { get; }

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _eventAggregator = eventAggregator;
            OpenCustomerDetailViewCommand = new DelegateCommand(OnOpenCustomerDetailView);
        }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        private void OnOpenCustomerDetailView()
        {
            _eventAggregator.GetEvent<OpenCustomerDetailViewEvent>().Publish(Id);
        }
    }
}
