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

    public SaleOrderInvoicePageViewModel GetPagedInvoices(InvoiceSearchViewModel search, int page)
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

      List<SaleOrderInvoice> invoiceList = invoices
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToList();

      List<SaleOrderInvoiceViewModel> invoiceVms = SaleOrderInvoiceViewModelFactory
        .BuildList(invoiceList);

      foreach (SaleOrderInvoiceViewModel invoice in invoiceVms)
      {
        string businessName = _context
          .SaleOrders
          .Where(so => so.Id == invoice.SaleOrderId)
          .Select(so => so.Customer.Name)
          .First();

        decimal discount = _context
          .SaleOrders
          .Where(po => po.Id == invoice.SaleOrderId)
          .Select(po => po.Discount)
          .First();

        invoice.Discount = discount;

        invoice.Business.Name = businessName;
      }

      SaleOrderInvoicePageViewModel invoicePageVm = new SaleOrderInvoicePageViewModel
      {
        Invoices = invoiceVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.SaleOrderInvoices.Count(i => !i.IsDeleted)
        }
      };

      return invoicePageVm;
    }

    public IEnumerable<SaleOrderInvoiceViewModel> GetAllInvoices()
    {
      IQueryable<SaleOrderInvoice> invoices = _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .AsNoTracking()
        .Where(i => !i.IsDeleted);

      List<SaleOrderInvoiceViewModel> invoiceVms = SaleOrderInvoiceViewModelFactory
        .BuildList(invoices.ToList());

      return invoiceVms;
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

      Address address = order
        .Customer
        .Addresses
        .First();

      viewModel.Business.AddressLine1 = address.AddressLine1;
      viewModel.Business.AddressLine2 = address.AddressLine2;
      viewModel.Business.Area = address.Area;
      viewModel.Business.City = address.City;
      viewModel.Business.PostCode = address.PostCode;
      viewModel.Business.Country = address.Country;

      viewModel.Business.ContactFullName =
        $"{order.Customer.Contacts.First().FirstName} {order.Customer.Contacts.First().LastName}";

      return viewModel;
    }

    public void AddInvoice(SaleOrderInvoiceViewModel invoice)
    {
      SaleOrder saleOrder = _context
        .SaleOrders
        .Include(po => po.LineItems)
        .ThenInclude(li => li.Product)
        .FirstOrDefault(po => po.Id == invoice.SaleOrderId);

      if (invoice.Discount > invoice.Amount)
      {
        throw new DiscountTooHighException();
      }

      if (saleOrder != null)
      {
        saleOrder.Discount = invoice.Discount;

        SaleOrderInvoice saleOrderInvoice = SaleOrderInvoiceFactory
          .Build(saleOrder, invoice);

        _context.SaleOrderInvoices.Add(saleOrderInvoice);
      }

      _context.SaveChanges();
    }

    public void UpdateInvoice(SaleOrderInvoiceViewModel invoiceVm)
    {
      SaleOrderInvoice saleOrderInvoice = _context
        .SaleOrderInvoices
        .Include(i => i.Payments)
        .Include(i => i.Returns)
        .First(i => i.Id == invoiceVm.Id && !i.IsDeleted);

      SaleOrderInvoiceFactory
        .MapViewModelToInvoice(invoiceVm, saleOrderInvoice);

      _context.Entry(saleOrderInvoice).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteInvoice(int id)
    {
      PurchaseOrderInvoice purchaseOrderInvoice = _context
        .PurchaseOrderInvoices
        .Include(i => i.Payments)
        .First(i => i.Id == id && !i.IsDeleted);

      purchaseOrderInvoice.IsDeleted = true;

      _context.Entry(purchaseOrderInvoice).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public InvoiceCustomerContactDetailsViewModel GetCustomerContactDetails(int invoiceId)
    {
      InvoiceCustomerContactDetailsViewModel customerContactDetails = new InvoiceCustomerContactDetailsViewModel();

      SaleOrderInvoice saleOrderInvoice = _context
        .SaleOrderInvoices
        .First(i => i.Id == invoiceId);

      SaleOrder saleOrder = _context
        .SaleOrders
        .Include(so => so.Customer)
        .ThenInclude(c => c.Contacts)
        .First(so => so.Id == saleOrderInvoice.SaleOrderId);

      customerContactDetails.Email = saleOrder.Customer.Email;

      customerContactDetails.Name = saleOrder.Customer.Contacts.FirstOrDefault()?.FirstName + " " +
                                    saleOrder.Customer.Contacts.FirstOrDefault()?.LastName;

      return customerContactDetails;
    }

    public async Task<decimal> GetRemainingCustomerCreditAsync(int orderId)
    {
      SaleOrder saleOrder = await _context.SaleOrders.Where(so => so.Id == orderId).FirstAsync();

      int customerId = saleOrder.CustomerId;

      Customer customer = await _context.Customers.FindAsync(customerId);

      decimal remainingCredit = customer.AllowedCredit - customer.CreditUsed;

      return remainingCredit;
    }
  }
}
