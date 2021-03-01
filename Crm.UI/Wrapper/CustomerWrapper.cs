using Crm.Model;
using System;
using System.Collections.Generic;

namespace Crm.UI.Wrapper
{
    public class CustomerWrapper : ModelWrapper<Customer>
    {
        public CustomerWrapper(Customer model) : base(model)
        {

        }

        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Code
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int? CustomerTypeId 
        { 
            get { return GetValue<int?>(); } 
            set { SetValue(value); }
        }
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (string.Equals(Name, "Volvo", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Volvo is not a valid customer";
                    }
                    break;
            }
        }
    }
}
