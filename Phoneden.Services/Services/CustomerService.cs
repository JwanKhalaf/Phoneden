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

    public CustomerService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public CustomerPageViewModel GetPagedCustomers(bool showDeleted, int page = 1)
    {
      IQueryable<Customer> customers = _context.Customers.AsNoTracking().AsQueryable();
      if (!showDeleted)
      {
        customers = customers.Where(c => !c.IsDeleted);
      }

      List<Customer> customerList = customers.OrderByDescending(s => s.CreatedOn).Skip(_recordsPerPage * (page - 1)).Take(_recordsPerPage).ToList();
      IEnumerable<CustomerViewModel> customerVms = CustomerViewModelFactory.BuildList(customerList);
      CustomerPageViewModel customerPagedVm = new CustomerPageViewModel
      {
        Customers = customerVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Customers.Count(c => !c.IsDeleted)
        }
      };
      return customerPagedVm;
    }

    public IEnumerable<CustomerViewModel> GetAllCustomers()
    {
      IQueryable<Customer> customers = _context.Customers
        .AsNoTracking()
        .Where(c => !c.IsDeleted);
      IEnumerable<CustomerViewModel> customerVms = CustomerViewModelFactory.BuildList(customers.ToList());
      return customerVms;
    }

    public async Task<CustomerViewModel> GetCustomerAsync(int id)
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

      CustomerViewModel customerViewModel = CustomerViewModelFactory
        .Build(customer);

      return customerViewModel;
    }

    public void AddCustomer(CustomerViewModel customerVm)
    {
      Customer customer = CustomerFactory.BuildNewCustomer(customerVm);
      _context.Customers.Add(customer);
      _context.SaveChanges();
    }

    public void UpdateCustomer(CustomerViewModel customerVm)
    {
      Customer customer = _context
        .Customers
        .First(c => c.Id == customerVm.Id && !c.IsDeleted);

      CustomerFactory.MapViewModelToCustomer(customerVm, customer);
      _context.Entry(customer).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
      Customer customer = _context
        .Customers
        .Include(c => c.Addresses)
        .Include(c => c.Contacts)
        .Where(c => !c.IsDeleted)
        .First(c => c.Id == id);

      customer.IsDeleted = true;

      foreach (Address address in customer.Addresses)
      {
        address.IsDeleted = true;
      }

      foreach (Contact contact in customer.Contacts)
      {
        contact.IsDeleted = true;
      }

      _context.SaveChanges();
    }
  }
}
