namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class PartnerFactory
  {
    public static ICollection<Partner> BuildNewPartnerCollectionFromViewModel(IList<PartnerViewModel> partners)
    {
      return partners?.Select(BuildNewPartnerFromViewModel).ToList();
    }

    public static Partner BuildNewPartnerFromViewModel(PartnerViewModel partnerViewModel)
    {
      if (partnerViewModel == null)
      {
        return null;
      }

      return new Partner
      {
        Title = partnerViewModel.Title,
        FirstName = partnerViewModel.FirstName,
        LastName = partnerViewModel.LastName,
        Phone = partnerViewModel.Phone,
        Email = partnerViewModel.Email,
        AddressLine1 = partnerViewModel.AddressLine1,
        AddressLine2 = partnerViewModel.AddressLine2,
        Area = partnerViewModel.Area,
        City = partnerViewModel.City,
        County = partnerViewModel.County,
        Country = partnerViewModel.Country,
        CreatedOn = DateTime.UtcNow
      };
    }

    public static void MapViewModelToPartner(PartnerViewModel partnerVm, Partner partner)
    {
      if (partnerVm == null)
      {
        throw new ArgumentNullException(nameof(partnerVm));
      }

      if (partner == null)
      {
        throw new ArgumentNullException(nameof(partner));
      }

      partner.Title = partnerVm.Title;
      partner.FirstName = partnerVm.FirstName;
      partner.LastName = partnerVm.LastName;
      partner.Phone = partnerVm.Phone;
      partner.Email = partnerVm.Email;
      partner.IsDeleted = partnerVm.IsDeleted;
      partner.AddressLine1 = partnerVm.AddressLine1;
      partner.AddressLine2 = partnerVm.AddressLine2;
      partner.Area = partnerVm.Area;
      partner.City = partnerVm.City;
      partner.PostCode = partnerVm.PostCode;
      partner.County = partnerVm.County;
      partner.Country = partnerVm.Country;
      partner.ModifiedOn = DateTime.UtcNow;
    }
  }
}
