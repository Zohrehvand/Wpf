using Crm.Model;

namespace Crm.UI.Wrapper
{
    public class CustomerContactWrapper : ModelWrapper<CustomerContact>
    {
        public CustomerContactWrapper(CustomerContact model) : base(model)
        {

        }
        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
