namespace Phoneden.Entities
{
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Partners")]
  public class Partner : Person
  {
    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string Area { get; set; }

    public string City { get; set; }

    public string County { get; set; }

    public string PostCode { get; set; }

    public string Country { get; set; }
  }
}
