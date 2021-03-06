using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Crm.Model
{
    public class Customer
    {
        public Customer()
        {
            CustomerContacts = new Collection<CustomerContact>();
            Campaigns = new Collection<Campaign>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public int? CustomerTypeId { get; set; }
        public CustomerType CustomerType { get; set; }
        public ICollection<CustomerContact> CustomerContacts { get; set; }

        public ICollection<Campaign> Campaigns { get; set; }
    }
}
