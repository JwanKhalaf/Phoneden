namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IQualityService
  {
    QualityPageViewModel GetPagedQualities(bool showDeleted, int page);

    IEnumerable<QualityViewModel> GetAllQualities();

    QualityViewModel GetQuality(int id);

    void AddQuality(QualityViewModel qualityVm);

    void UpdateQuality(QualityViewModel qualityVm);

    void DeleteQuality(int id);

    bool QualityIsInUse(int id);
  }
}
