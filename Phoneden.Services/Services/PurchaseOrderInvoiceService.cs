namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Entities.Shared;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class PurchaseOrderInvoiceService : IPurchaseOrderInvoiceService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public PurchaseOrderInvoiceService(
      IPaginationConfiguration paginationSettings,
      PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<PurchaseOrderInvoicePageViewModel> GetPagedInvoicesAsync(InvoiceSearchViewModel search, int page = 1)
    {
      if (search.SearchTermHasChanged())
      {
        page = 1;
      }

      IQueryable<PurchaseOrderInvoice> invoices = _context
        .PurchaseOrderInvoices
        .Include(i => i.Payments)
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

      List<PurchaseOrderInvoice> invoiceList = await invoices
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      List<PurchaseOrderInvoiceViewModel> invoiceVms = PurchaseOrderInvoiceViewModelFactory
        .BuildList(invoiceList);

      foreach (PurchaseOrderInvoiceViewModel invoice in invoiceVms)
      {
        string businessName = _context
          .PurchaseOrders
          .Where(so => so.Id == invoice.PurchaseOrderId)
          .Select(so => so.Supplier.Name)
          .First();

        decimal discount = _context
          .PurchaseOrders
          .Where(po => po.Id == invoice.PurchaseOrderId)
          .Select(po => po.Discount)
          .First();

        invoice.Discount = discount;

        invoice.Business.Name = businessName;
      }

      PurchaseOrderInvoicePageViewModel invoicePageVm = new PurchaseOrderInvoicePageViewModel
      {
        Invoices = invoiceVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.PurchaseOrderInvoices.Count(i => !i.IsDeleted)
        }
      };

      return invoicePageVm;
    }

    public async Task<IEnumerable<PurchaseOrderInvoiceViewModel>> GetAllInvoicesAsync()
    {
      IQueryable<PurchaseOrderInvoice> invoices = _context
        .PurchaseOrderInvoices
        .Include(i => i.Payments)
        .AsNoTracking()
        .Where(i => !i.IsDeleted);

      List<PurchaseOrderInvoiceViewModel> invoiceVms = PurchaseOrderInvoiceViewModelFactory
        .BuildList(await invoices.ToListAsync());

      return invoiceVms;
    }

    public async Task<PurchaseOrderInvoiceViewModel> GetInvoiceAsync(int id)
    {
      PurchaseOrderInvoice purchaseOrderInvoice = await _context
        .PurchaseOrderInvoices
        .Include(i => i.InvoicedLineItems)
        .Include(i => i.Payments)
        .FirstAsync(i => i.Id == id);

      PurchaseOrderInvoiceViewModel viewModel = PurchaseOrderInvoiceViewModelFactory
        .Build(purchaseOrderInvoice);

      PurchaseOrder order = await _context
        .PurchaseOrders
        .Include(so => so.Supplier)
        .ThenInclude(c => c.Contacts)
        .Include(so => so.Supplier)
        .ThenInclude(c => c.Addresses)
        .FirstOrDefaultAsync(so => so.Id == purchaseOrderInvoice.PurchaseOrderId);

      if (order == null)
      {
        throw new NullReferenceException();
      }

      viewModel.Business.Name = order.Supplier.Name;

      Address address = order
        .Supplier
        .Addresses
        .First();

      viewModel.Business.AddressLine1 = address.AddressLine1;
      viewModel.Business.AddressLine2 = address.AddressLine2;
      viewModel.Business.Area = address.Area;
      viewModel.Business.City = address.City;
      viewModel.Business.PostCode = address.PostCode;
      viewModel.Business.Country = address.Country;

      viewModel.Business.ContactFullName =
        $"{order.Supplier.Contacts.First().FirstName} {order.Supplier.Contacts.First().LastName}";

      return viewModel;
    }

    public void AddInvoice(PurchaseOrderInvoiceViewModel invoiceVm)
    {
      PurchaseOrder purchaseOrder = _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .ThenInclude(li => li.Product)
        .FirstOrDefault(po => po.Id == invoiceVm.PurchaseOrderId);

      if (invoiceVm.Discount > invoiceVm.Amount)
      {
        throw new DiscountTooHighException();
      }

      if (purchaseOrder != null)
      {
        purchaseOrder.Discount = invoiceVm.Discount;

        PurchaseOrderInvoice purchaseOrderInvoice = PurchaseOrderInvoiceFactory
          .Build(purchaseOrder, invoiceVm);

        _context.PurchaseOrderInvoices.Add(purchaseOrderInvoice);
      }

      _context.SaveChanges();
    }

    public void UpdateInvoice(PurchaseOrderInvoiceViewModel invoiceVm)
    {
      PurchaseOrderInvoice purchaseOrderInvoice = _context
        .PurchaseOrderInvoices
        .Include(i => i.Payments)
        .First(i => i.Id == invoiceVm.Id && !i.IsDeleted);

      PurchaseOrderInvoiceFactory
        .MapViewModelToInvoice(invoiceVm, purchaseOrderInvoice);

      _context.Entry(purchaseOrderInvoice).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public async Task<bool> IsInvoiceUpToDate(int purchaseOrderId)
    {
      PurchaseOrder purchaseOrder = await _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .Include(po => po.Invoice)
        .AsNoTracking()
        .FirstOrDefaultAsync(po => po.Id == purchaseOrderId);

      if (purchaseOrder.Invoice != null)
      {
        PurchaseOrderInvoice invoice = await _context
          .PurchaseOrderInvoices
          .Include(i => i.InvoicedLineItems)
          .AsNoTracking()
          .FirstOrDefaultAsync(i => i.Id == purchaseOrder.Invoice.Id);

        if (purchaseOrder.LineItems.Count == invoice.InvoicedLineItems.Count)
        {
          List<bool> tempList = new List<bool>();

          for (int i = 0; i < purchaseOrder.LineItems.Count; i++)
          {
            PurchaseOrderLineItem purchaseOrderLine = purchaseOrder.LineItems.ToList()[i];
            PurchaseOrderInvoiceLineItem purchaseOrderInvoiceLine = invoice.InvoicedLineItems.ToList()[i];
            if (purchaseOrderLine.ProductId == purchaseOrderInvoiceLine.ProductId &&
                purchaseOrderLine.Quantity == purchaseOrderInvoiceLine.Quantity &&
                purchaseOrderLine.Price == purchaseOrderInvoiceLine.Price &&
                purchaseOrderLine.Currency == purchaseOrderInvoiceLine.Currency && purchaseOrderLine.ConversionRate ==
                purchaseOrderInvoiceLine.ConversionRate)
            {
              tempList.Add(true);
            }
            else
            {
              tempList.Add(false);
            }
          }

          return tempList.All(item => item);
        }

        return false;
      }

      throw new InvoiceNotFoundException($"Could not find invoice for Purchase Order Id: {purchaseOrder.Id}");
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

    public async Task<InvoiceSupplierContactDetailsViewModel> GetSupplierContactDetailsAsync(int invoiceId)
    {
      InvoiceSupplierContactDetailsViewModel customerContactDetails = new InvoiceSupplierContactDetailsViewModel();

      PurchaseOrderInvoice purchaseOrderInvoice = await _context
        .PurchaseOrderInvoices
        .FirstAsync(i => i.Id == invoiceId);

      PurchaseOrder purchaseOrder = await _context
        .PurchaseOrders
        .Include(so => so.Supplier)
        .ThenInclude(c => c.Contacts)
        .FirstAsync(so => so.Id == purchaseOrderInvoice.PurchaseOrderId);

      customerContactDetails.Email = purchaseOrder.Supplier.Email;

      customerContactDetails.Name = purchaseOrder.Supplier.Contacts.FirstOrDefault()?.FirstName + " " +
                                    purchaseOrder.Supplier.Contacts.FirstOrDefault()?.LastName;

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

    private void UpdateProductCost(int invoiceOrderId)
    {
      PurchaseOrder purchaseOrder = _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .First(po => po.Id == invoiceOrderId);

      decimal shippingCostInGbp = purchaseOrder.ShippingCurrency == Currency.Gbp
        ? purchaseOrder.ShippingCost
        : purchaseOrder.ShippingCost / purchaseOrder.ShippingConversionRate;

      int totalQuantity = purchaseOrder.LineItems.Sum(lineItem => lineItem.Quantity);

      decimal totalLogisticsCost = purchaseOrder.Vat + purchaseOrder.ImportDuty + shippingCostInGbp;

      decimal logisticsCostPerUnit = totalLogisticsCost / totalQuantity;

      foreach (PurchaseOrderLineItem lineItem in purchaseOrder.LineItems)
      {
        Product product = _context
          .Products
          .First(p => p.Id == lineItem.ProductId && !p.IsDeleted);

        decimal lineItemPrice = lineItem.Currency == Currency.Gbp
          ? lineItem.Price
          : lineItem.Price / lineItem.ConversionRate;

        product.UnitCostPrice = lineItemPrice + logisticsCostPerUnit;

        _context.Products.Update(product);
        _context.SaveChanges();
      }
    }
  }
}
