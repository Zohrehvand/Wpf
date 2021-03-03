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
        private string _detailViewModelName;
        public ICommand OpenDetailViewCommand { get; }
        public int Id { get; }

        public NavigationItemViewModel(int id, string displayMember,string detailViewModelName, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _detailViewModelName = detailViewModelName;
            _eventAggregator = eventAggregator;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
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

        private void OnOpenDetailViewExecute()
        {
            var args = new OpenDetailViewEventArgs
            {
                Id = Id,
                ViewModelName = _detailViewModelName
            };
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(args);
        }
    }
}
