using System.ComponentModel.DataAnnotations;

namespace Crm.Model
{
  public class Customer
  {
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }
  }
}
