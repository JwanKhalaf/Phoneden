namespace Phoneden.Entities
{
  using System;
  using Shared;

  public class Brand : IEntity
  {
    public Brand()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
