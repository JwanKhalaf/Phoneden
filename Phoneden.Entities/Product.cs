namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public enum Colour
  {
    None = 0,
    Clear = 1,
    Black = 2,
    White = 3,
    Grey = 4,
    Silver = 5,
    Gold = 6,
    RoseGold = 7,
    Blue = 8,
    Green = 9,
    Red = 10,
    Purple = 11,
    Pink = 12,
    Brown = 13
  }

  public class Product : IEntity
  {
    public Product()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Sku { get; set; }

    public string Barcode { get; set; }

    public string Description { get; set; }

    public Colour Colour { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal UnitCostPrice { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal UnitSellingPrice { get; set; }

    public int AlertThreshold { get; set; }

    public string ImagePath { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public int QualityId { get; set; }

    public Category Category { get; set; }

    public Brand Brand { get; set; }

    public Quality Quality { get; set; }
  }
}
