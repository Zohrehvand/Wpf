using System.ComponentModel.DataAnnotations;

namespace Crm.Model
{
    public class CustomerContact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Phone]
        [Required]
        public string Number { get; set; }
    }
}
