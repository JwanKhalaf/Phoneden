namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class QualityFactory
  {
    public static Quality BuildNewQuality(QualityViewModel qualityVm)
    {
      if (qualityVm == null)
      {
        throw new ArgumentNullException(nameof(qualityVm));
      }

      Quality quality = new Quality
      {
        Name = qualityVm.Name,
        CreatedOn = DateTime.UtcNow
      };
      return quality;
    }

    public static void MapViewModelToQuality(QualityViewModel qualityVm, Quality quality)
    {
      if (qualityVm == null)
      {
        throw new ArgumentNullException(nameof(qualityVm));
      }

      if (quality == null)
      {
        throw new ArgumentNullException(nameof(quality));
      }

      quality.Name = qualityVm.Name;
      quality.ModifiedOn = DateTime.UtcNow;
    }
  }
}
