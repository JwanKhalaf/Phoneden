namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class PartnerViewModelFactory
  {
    public static List<PartnerViewModel> BuildListOfPartnerViewModels(ICollection<Partner> partners)
    {
      if (partners == null)
      {
        throw new ArgumentNullException(nameof(partners));
      }

      return partners.Select(BuildPartnerViewModel).ToList();
    }

    public static PartnerViewModel BuildPartnerViewModel(Partner partner)
    {
      if (partner == null)
      {
        throw new ArgumentNullException(nameof(partner));
      }

      PartnerViewModel viewModel = new PartnerViewModel
      {
        Id = partner.Id,
        Title = partner.Title,
        FirstName = partner.FirstName,
        LastName = partner.LastName,
        Phone = partner.Phone,
        Email = partner.Email,
        AddressLine1 = partner.AddressLine1,
        AddressLine2 = partner.AddressLine2,
        Area = partner.Area,
        City = partner.City,
        PostCode = partner.PostCode,
        County = partner.County,
        Country = partner.Country,
        IsDeleted = partner.IsDeleted
      };
      return viewModel;
    }
  }
}
