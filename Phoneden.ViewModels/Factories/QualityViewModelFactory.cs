namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class QualityViewModelFactory
  {
    public static List<QualityViewModel> BuildListOfQualityViewModels(IEnumerable<Quality> qualityList)
    {
      if (qualityList == null)
      {
        throw new ArgumentNullException(nameof(qualityList));
      }

      return qualityList.Select(BuildQualityViewModel).ToList();
    }

    public static QualityViewModel BuildQualityViewModel(Quality quality)
    {
      if (quality == null)
      {
        throw new ArgumentNullException(nameof(quality));
      }

      QualityViewModel qualityVm = new QualityViewModel
      {
        Id = quality.Id,
        Name = quality.Name,
        IsDeleted = quality.IsDeleted,
        CreatedOn = quality.CreatedOn
      };
      return qualityVm;
    }
  }
}
