namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public abstract class LineItem : IEntity
  {
    protected LineItem()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Price { get; set; }

    public Currency Currency { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal ConversionRate { get; set; }

    public string Quality { get; set; }

    public string Colour { get; set; }

    public int Quantity { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public decimal CalculateTotal()
    {
      return Price * Quantity;
    }
  }
}
