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

  public class QualityService : IQualityService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public QualityService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public QualityPageViewModel GetPagedQualities(bool showDeleted, int page)
    {
      IQueryable<Quality> qualities = _context.Qualities
        .AsNoTracking()
        .AsQueryable();
      if (!showDeleted)
      {
        qualities = qualities.Where(s => !s.IsDeleted);
      }

      List<QualityViewModel> qualityVms = QualityViewModelFactory.BuildListOfQualityViewModels(qualities);
      QualityPageViewModel qualityPageVm = new QualityPageViewModel
      {
        Qualities = qualityVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Qualities.Count(b => !b.IsDeleted)
        }
      };
      return qualityPageVm;
    }

    public IEnumerable<QualityViewModel> GetAllQualities()
    {
      IQueryable<Quality> qualities = _context.Qualities
        .AsNoTracking()
        .Where(q => !q.IsDeleted);
      List<QualityViewModel> qualitiesVm = QualityViewModelFactory.BuildListOfQualityViewModels(qualities.ToList());
      return qualitiesVm;
    }

    public QualityViewModel GetQuality(int id)
    {
      Quality quality = _context.Qualities
        .AsNoTracking()
        .First(q => q.Id == id);
      QualityViewModel qualityVm = QualityViewModelFactory.BuildQualityViewModel(quality);
      return qualityVm;
    }

    public void AddQuality(QualityViewModel qualityVm)
    {
      Quality quality = QualityFactory.BuildNewQuality(qualityVm);
      _context.Qualities.Add(quality);
      _context.SaveChanges();
    }

    public void UpdateQuality(QualityViewModel qualityVm)
    {
      Quality quality = _context.Qualities.First(q => q.Id == qualityVm.Id && !q.IsDeleted);
      MapFromViewModel(qualityVm, quality);
      _context.Entry(quality).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteQuality(int id)
    {
      Quality quality = _context.Qualities.First(q => q.Id == id && !q.IsDeleted);
      quality.IsDeleted = true;
      _context.Entry(quality).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool QualityIsInUse(int id)
    {
      return _context.Products.Any(p => p.QualityId == id);
    }

    private static void MapFromViewModel(QualityViewModel viewModel, Quality quality)
    {
      quality.Name = viewModel.Name;
      quality.IsDeleted = viewModel.IsDeleted;
    }
  }
}
