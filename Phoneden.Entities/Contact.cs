namespace Phoneden.Entities
{
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Contacts")]
  public class Contact : Person
  {
    public string Department { get; set; }

    public int BusinessId { get; set; }

    public Business Business { get; set; }
  }
}
