namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IPartnerService
  {
    PartnerPageViewModel GetPagedPartners(bool showDeleted, int page);

    List<PartnerViewModel> GetAllPartners();

    PartnerViewModel GetPartner(int id);

    void AddPartner(PartnerViewModel partnerVm);

    void UpdatePartner(PartnerViewModel partnerVm);

    void DeletePartner(int id);
  }
}
