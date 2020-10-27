namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using Shared;

  public class Category : IEntity
  {
    public Category()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ParentCategoryId { get; set; }

    public Category ParentCategory { get; set; }

    public ICollection<Product> Products { get; set; }
  }
}
