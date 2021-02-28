using Prism.Events;

namespace Crm.UI.Event
{
    public class AfterCustomerSavedEvent : PubSubEvent<AfterCustomerSavedEventArgs>
    {

    }

    public class AfterCustomerSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
