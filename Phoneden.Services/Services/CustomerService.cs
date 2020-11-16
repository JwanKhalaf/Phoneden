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
  using ViewModels;

  public class CustomerService : ICustomerService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public CustomerService(
      PdContext context,
      IPaginationConfiguration paginationSettings)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<CustomerPageViewModel> GetPagedCustomersAsync(
      bool showDeleted,
      int page = 1)
    {
      IQueryable<Customer> customers = _context
        .Customers
        .AsNoTracking()
        .AsQueryable();

      if (!showDeleted)
      {
        customers = customers.Where(c => !c.IsDeleted);
      }

      List<Customer> customerList = await customers
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      IEnumerable<CustomerViewModel> customerVms = CustomerViewModelFactory.BuildList(customerList);

      CustomerPageViewModel viewModel = new CustomerPageViewModel
      {
        Customers = customerVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Customers.Count(c => !c.IsDeleted)
        }
      };
      return viewModel;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
    {
      IQueryable<Customer> customers = _context
        .Customers
        .AsNoTracking()
        .Where(c => !c.IsDeleted);

      IEnumerable<CustomerViewModel> viewModel = CustomerViewModelFactory
        .BuildList(await customers.ToListAsync());

      return viewModel;
    }

    public async Task<CustomerViewModel> GetCustomerAsync(
      int id)
    {
      Customer customer = await _context.Customers
        .Include(c => c.SaleOrders)
        .ThenInclude(so => so.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(c => c.SaleOrders)
        .ThenInclude(so => so.Invoice)
        .ThenInclude(i => i.Returns)
        .ThenInclude(r => r.Product)
        .Include(c => c.SaleOrders)
        .ThenInclude(so => so.LineItems)
        .Include(c => c.Addresses)
        .Include(c => c.Contacts)
        .AsNoTracking()
        .FirstAsync(c => c.Id == id);

      CustomerViewModel viewModel = CustomerViewModelFactory
        .Build(customer);

      return viewModel;
    }

    public async Task AddCustomerAsync(
      CustomerViewModel viewModel)
    {
      Customer customer = CustomerFactory
        .BuildNewCustomer(viewModel);

      _context.Customers.Add(customer);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(
      CustomerViewModel viewModel)
    {
      Customer customer = await _context
        .Customers
        .FirstAsync(c => c.Id == viewModel.Id && !c.IsDeleted);

      CustomerFactory.MapViewModelToCustomer(viewModel, customer);

      _context.Entry(customer).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(
      int id)
    {
      Customer customer = await _context
        .Customers
        .Include(c => c.Addresses)
        .Include(c => c.Contacts)
        .Where(c => !c.IsDeleted)
        .FirstAsync(c => c.Id == id);

      customer.IsDeleted = true;

      foreach (CustomerAddress address in customer.Addresses)
      {
        address.IsDeleted = true;
      }

      foreach (CustomerContact contact in customer.Contacts)
      {
        contact.IsDeleted = true;
      }

      await _context.SaveChangesAsync();
    }
  }
}
