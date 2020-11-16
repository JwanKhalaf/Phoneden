namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class CustomerViewModelFactory
  {
    public static IEnumerable<CustomerViewModel> BuildList(IEnumerable<Customer> customers)
    {
      if (customers == null)
      {
        throw new ArgumentNullException(nameof(customers));
      }

      return customers.Select(Build).ToList();
    }

    public static CustomerViewModel Build(Customer customer)
    {
      if (customer == null)
      {
        throw new ArgumentNullException(nameof(customer));
      }

      CustomerViewModel viewModel = new CustomerViewModel();
      viewModel.Id = customer.Id;
      viewModel.Name = customer.Name;
      viewModel.Code = customer.Code;
      viewModel.Description = customer.Description;
      viewModel.Phone = customer.Phone;
      viewModel.Website = customer.Website;
      viewModel.Email = customer.Email;
      viewModel.IsDeleted = customer.IsDeleted;
      viewModel.AllowedCredit = customer.AllowedCredit;
      viewModel.CreditUsed = customer.CreditUsed;
      viewModel.NumberOfDaysAllowedToBeOnMaxedOutCredit = customer.NumberOfDaysAllowedToBeOnMaxedOutCredit;
      viewModel.NumberOfDaysSinceCreditUsage = CalculateNumberOfDaysSinceCreditUsage(customer);
      viewModel.NumberOfDaysCreditIsOverdue = CalculateNumberOfDaysCreditIsOverdue(viewModel.NumberOfDaysSinceCreditUsage, viewModel.NumberOfDaysAllowedToBeOnMaxedOutCredit);

      viewModel.Addresses = CustomerAddressViewModelFactory
        .BuildList(customer.Addresses, false);

      viewModel.Contacts = CustomerContactViewModelFactory
        .BuildListOfContactViewModels(customer.Contacts, false);

      if (customer.SaleOrders == null)
      {
        return viewModel;
      }

      viewModel.SaleOrders = SaleOrderViewModelFactory
        .BuildList(customer.SaleOrders);

      viewModel.Returns = SaleOrderReturnViewModelFactory
        .BuildList(customer.SaleOrders.SelectMany(so => so.Invoice.Returns));

      return viewModel;
    }

    private static int CalculateNumberOfDaysCreditIsOverdue(int numberOfDaysSinceCreditUsage, int numberOfDaysAllowedToBeOnMaxedOutCredit)
    {
      if (numberOfDaysSinceCreditUsage > numberOfDaysAllowedToBeOnMaxedOutCredit)
      {
        return numberOfDaysSinceCreditUsage - numberOfDaysAllowedToBeOnMaxedOutCredit;
      }

      return 0;
    }

    private static int CalculateNumberOfDaysSinceCreditUsage(Customer customer)
    {
      if (customer.SaleOrders == null)
      {
        return 0;
      }

      List<SaleOrder> customerOrders = customer
        .SaleOrders
        .ToList();

      if (customerOrders.Any(o => o.Invoice.AmountToBePaidOnCredit > 0))
      {
        List<SaleOrderInvoice> invoicesWithCreditUsage = customerOrders
          .Where(co => co.Invoice.AmountToBePaidOnCredit > 0)
          .Select(so => so.Invoice)
          .OrderByDescending(invoice => invoice.CreatedOn)
          .ToList();

        DateTime dateOfOldestOrder = invoicesWithCreditUsage.First().CreatedOn;
        DateTime currentDateAndTime = DateTime.UtcNow;
        int numberOfDaysSinceFirstCreditUsed = (currentDateAndTime - dateOfOldestOrder).Days;
        return numberOfDaysSinceFirstCreditUsed;
      }

      return 0;
    }
  }
}
