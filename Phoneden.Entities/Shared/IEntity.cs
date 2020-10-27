namespace Phoneden.Entities.Shared
{
  using System;

  public interface IEntity
  {
    bool IsDeleted { get; set; }

    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
  }
}
