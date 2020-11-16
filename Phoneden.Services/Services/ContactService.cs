namespace Phoneden.Services
{
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class ContactService : IContactService
  {
    private readonly PdContext _context;

    public ContactService(PdContext context)
    {
      _context = context;
    }

    public ContactViewModel GetContact(int id, bool isSupplierContact)
    {
      SupplierContact supplierContact = _context.SupplierContacts.AsNoTracking().First(c => c.Id == id);
      ContactViewModel addressVm = SupplierContactViewModelFactory.BuildContactViewModel(supplierContact, isSupplierContact);
      return addressVm;
    }

    public void AddContact(ContactViewModel contactVm)
    {
      SupplierContact supplierContact = SupplierContactFactory.BuildNewContactFromViewModel(contactVm);
      _context.SupplierContacts.Add(supplierContact);
      _context.SaveChanges();
    }

    public void UpdateContact(ContactViewModel contactVm)
    {
      SupplierContact supplierContact = _context.SupplierContacts.Where(c => !c.IsDeleted).First(c => c.Id == contactVm.Id);
      SupplierContactFactory.MapViewModelToContact(contactVm, supplierContact);
      _context.Entry(supplierContact).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteContact(int id, bool isSupplierContact)
    {
      SupplierContact supplierContact = _context.SupplierContacts.Where(c => !c.IsDeleted).First(c => c.Id == id);
      supplierContact.IsDeleted = true;
      _context.Entry(supplierContact).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool CanContactBeDeleted(int id, bool isSupplierContact)
    {
      SupplierContact supplierContact = _context.SupplierContacts.Where(c => !c.IsDeleted).First(c => c.Id == id);
      if (isSupplierContact)
      {
        Supplier supplier = _context.Suppliers
                        .Include(s => s.Contacts)
                        .Where(s => !s.IsDeleted)
                        .First(s => s.Id == supplierContact.SupplierId);
        if (supplier.Contacts.Where(a => !a.IsDeleted).ToList().Count > 1)
        {
          return true;
        }
      }
      else
      {
        Customer customer = _context.Customers
                        .Include(c => c.Contacts)
                        .Where(c => !c.IsDeleted)
                        .First(c => c.Id == supplierContact.SupplierId);
        if (customer.Contacts.Where(a => !a.IsDeleted).ToList().Count > 1)
        {
          return true;
        }
      }

      return false;
    }
  }
}
