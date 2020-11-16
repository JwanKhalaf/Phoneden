namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using SaleOrders;
  using ViewModels;

  public class SaleOrderInvoiceService : ISaleOrderInvoiceService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public SaleOrderInvoiceService(
      IPaginationConfiguration paginationSettings,
      PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<SaleOrderInvoicePageViewModel> GetPagedInvoicesAsync(
      InvoiceSearchViewModel search,
      int page)
    {
      if (search.SearchTermHasChanged())
      {
        page = 1;
      }

      IQueryable<SaleOrderInvoice> invoices = _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .ThenInclude(r => r.Product)
        .AsNoTracking()
        .AsQueryable();

      if (!search.ShowDeleted)
      {
        invoices = invoices
          .Where(i => !i.IsDeleted);
      }

      if (!string.IsNullOrEmpty(search.SearchTerm) && search.IsNumeric())
      {
        int searchInvoiceId = int.Parse(search.SearchTerm);

        invoices = invoices
          .Where(i => i.Id == searchInvoiceId);
      }
      else if (!string.IsNullOrEmpty(search.SearchTerm) && !search.IsNumeric())
      {
        invoices = invoices
          .Where(i => i.Id == 0);
      }

      List<SaleOrderInvoice> invoiceList = await invoices
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      List<SaleOrderInvoiceViewModel> invoiceVms = SaleOrderInvoiceViewModelFactory
        .BuildList(invoiceList);

      foreach (SaleOrderInvoiceViewModel invoice in invoiceVms)
      {
        string businessName = await _context
          .SaleOrders
          .Where(so => so.Id == invoice.SaleOrderId)
          .Select(so => so.Customer.Name)
          .FirstAsync();

        decimal discount = await _context
          .SaleOrders
          .Where(po => po.Id == invoice.SaleOrderId)
          .Select(po => po.Discount)
          .FirstAsync();

        invoice.Discount = discount;

        invoice.Business.Name = businessName;
      }

      SaleOrderInvoicePageViewModel viewModel = new SaleOrderInvoicePageViewModel
      {
        Invoices = invoiceVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.SaleOrderInvoices.Count(i => !i.IsDeleted)
        }
      };

      return viewModel;
    }

    public async Task<IEnumerable<SaleOrderInvoiceViewModel>> GetAllInvoicesAsync()
    {
      IQueryable<SaleOrderInvoice> invoices = _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .AsNoTracking()
        .Where(i => !i.IsDeleted);

      List<SaleOrderInvoiceViewModel> viewModel = SaleOrderInvoiceViewModelFactory
        .BuildList(await invoices.ToListAsync());

      return viewModel;
    }

    public async Task<SaleOrderInvoiceViewModel> GetInvoiceAsync(int id)
    {
      SaleOrderInvoice saleOrderInvoice = await _context
        .SaleOrderInvoices
        .Include(i => i.InvoicedLineItems)
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .ThenInclude(r => r.Product)
        .FirstAsync(i => i.Id == id);

      SaleOrderInvoiceViewModel viewModel = SaleOrderInvoiceViewModelFactory
        .Build(saleOrderInvoice);

      SaleOrder order = await _context
        .SaleOrders
        .Include(so => so.Customer)
        .ThenInclude(c => c.Contacts)
        .Include(so => so.Customer)
        .ThenInclude(c => c.Addresses)
        .FirstOrDefaultAsync(so => so.Id == saleOrderInvoice.SaleOrderId);

      if (order == null)
      {
        throw new NullReferenceException();
      }

      viewModel.Business.Name = order.Customer.Name;

      CustomerAddress customerAddress = order
        .Customer
        .Addresses
        .First();

      viewModel.Business.AddressLine1 = customerAddress.AddressLine1;
      viewModel.Business.AddressLine2 = customerAddress.AddressLine2;
      viewModel.Business.Area = customerAddress.Area;
      viewModel.Business.City = customerAddress.City;
      viewModel.Business.PostCode = customerAddress.PostCode;
      viewModel.Business.Country = customerAddress.Country;

      viewModel.Business.ContactFullName =
        $"{order.Customer.Contacts.First().FirstName} {order.Customer.Contacts.First().LastName}";

      return viewModel;
    }

    public async Task AddInvoiceAsync(SaleOrderInvoiceViewModel viewModel)
    {
      SaleOrder saleOrder = await _context
        .SaleOrders
        .Include(po => po.LineItems)
        .ThenInclude(li => li.Product)
        .FirstOrDefaultAsync(po => po.Id == viewModel.SaleOrderId);

      if (viewModel.Discount > viewModel.Amount)
      {
        throw new DiscountTooHighException();
      }

      if (saleOrder != null)
      {
        saleOrder.Discount = viewModel.Discount;

        SaleOrderInvoice saleOrderInvoice = SaleOrderInvoiceFactory
          .Build(saleOrder, viewModel);

        _context
          .SaleOrderInvoices
          .Add(saleOrderInvoice);
      }

      await _context.SaveChangesAsync();
    }

    public async Task UpdateInvoiceAsync(SaleOrderInvoiceViewModel viewModel)
    {
      SaleOrderInvoice saleOrderInvoice = await _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .FirstAsync(i => i.Id == viewModel.Id && !i.IsDeleted);

      SaleOrderInvoiceFactory
        .MapViewModelToInvoice(viewModel, saleOrderInvoice);

      _context.Entry(saleOrderInvoice).State = EntityState.Modified;

      await _context
        .SaveChangesAsync();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
      SaleOrderInvoice invoice = await _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .FirstAsync(i => i.Id == id && !i.IsDeleted);

      invoice.IsDeleted = true;

      _context.Entry(invoice).State = EntityState.Modified;

      await _context
        .SaveChangesAsync();
    }

    public async Task<InvoiceCustomerContactDetailsViewModel> GetCustomerContactDetailsAsync(int invoiceId)
    {
      InvoiceCustomerContactDetailsViewModel viewModel = new InvoiceCustomerContactDetailsViewModel();

      SaleOrderInvoice saleOrderInvoice = await _context
        .SaleOrderInvoices
        .FirstAsync(i => i.Id == invoiceId);

      SaleOrder saleOrder = await _context
        .SaleOrders
        .Include(so => so.Customer)
        .ThenInclude(c => c.Contacts)
        .FirstAsync(so => so.Id == saleOrderInvoice.SaleOrderId);

      viewModel.Email = saleOrder.Customer.Email;

      viewModel.Name = saleOrder.Customer.Contacts.FirstOrDefault()?.FirstName + " " +
                       saleOrder.Customer.Contacts.FirstOrDefault()?.LastName;

      return viewModel;
    }

    public async Task<decimal> GetRemainingCustomerCreditAsync(int orderId)
    {
      SaleOrder saleOrder = await _context
        .SaleOrders
        .Where(so => so.Id == orderId)
        .FirstAsync();

      int customerId = saleOrder.CustomerId;

      Customer customer = await _context
        .Customers
        .FindAsync(customerId);

      decimal remainingCredit = customer.AllowedCredit - customer.CreditUsed;

      return remainingCredit;
    }
  }
}
