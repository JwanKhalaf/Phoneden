namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class PartnerService : IPartnerService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public PartnerService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public PartnerPageViewModel GetPagedPartners(bool showDeleted, int page)
    {
      IQueryable<Partner> partners = _context.Partners.AsNoTracking().AsQueryable();
      if (!showDeleted)
      {
        partners = partners.Where(s => !s.IsDeleted);
      }

      List<Partner> partnerList = partners.OrderByDescending(s => s.CreatedOn).Skip(_recordsPerPage * (page - 1)).Take(_recordsPerPage).ToList();
      List<PartnerViewModel> partnerVms = PartnerViewModelFactory.BuildListOfPartnerViewModels(partnerList);
      PartnerPageViewModel partnerPageVm = new PartnerPageViewModel
      {
        Partners = partnerVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Suppliers.Count(s => !s.IsDeleted)
        }
      };
      return partnerPageVm;
    }

    public List<PartnerViewModel> GetAllPartners()
    {
      IQueryable<Partner> partners = _context.Partners.Where(p => !p.IsDeleted);
      List<PartnerViewModel> partnerVms = PartnerViewModelFactory.BuildListOfPartnerViewModels(partners.ToList());
      return partnerVms;
    }

    public PartnerViewModel GetPartner(int id)
    {
      Partner partner = _context.Partners.First(p => p.Id == id);
      PartnerViewModel partnerViewModel = PartnerViewModelFactory.BuildPartnerViewModel(partner);
      return partnerViewModel;
    }

    public void AddPartner(PartnerViewModel partnerVm)
    {
      Partner partner = PartnerFactory.BuildNewPartnerFromViewModel(partnerVm);
      _context.Partners.Add(partner);
      _context.SaveChanges();
    }

    public void UpdatePartner(PartnerViewModel partnerVm)
    {
      Partner partner = _context.Partners.Where(p => !p.IsDeleted).First(p => p.Id == partnerVm.Id);
      PartnerFactory.MapViewModelToPartner(partnerVm, partner);
      _context.Entry(partner).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeletePartner(int id)
    {
      Partner partner = _context.Partners.Where(p => !p.IsDeleted).First(p => p.Id == id);
      partner.IsDeleted = true;
      _context.Entry(partner).State = EntityState.Modified;
      _context.SaveChanges();
    }
  }
}
