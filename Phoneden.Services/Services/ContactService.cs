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
      Contact contact = _context.Contacts.AsNoTracking().First(c => c.Id == id);
      ContactViewModel addressVm = ContactViewModelFactory.BuildContactViewModel(contact, isSupplierContact);
      return addressVm;
    }

    public void AddContact(ContactViewModel contactVm)
    {
      Contact contact = ContactFactory.BuildNewContactFromViewModel(contactVm);
      _context.Contacts.Add(contact);
      _context.SaveChanges();
    }

    public void UpdateContact(ContactViewModel contactVm)
    {
      Contact contact = _context.Contacts.Where(c => !c.IsDeleted).First(c => c.Id == contactVm.Id);
      ContactFactory.MapViewModelToContact(contactVm, contact);
      _context.Entry(contact).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteContact(int id, bool isSupplierContact)
    {
      Contact contact = _context.Contacts.Where(c => !c.IsDeleted).First(c => c.Id == id);
      contact.IsDeleted = true;
      _context.Entry(contact).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool CanContactBeDeleted(int id, bool isSupplierContact)
    {
      Contact contact = _context.Contacts.Where(c => !c.IsDeleted).First(c => c.Id == id);
      if (isSupplierContact)
      {
        Supplier supplier = _context.Suppliers
                        .Include(s => s.Contacts)
                        .Where(s => !s.IsDeleted)
                        .First(s => s.Id == contact.BusinessId);
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
                        .First(c => c.Id == contact.BusinessId);
        if (customer.Contacts.Where(a => !a.IsDeleted).ToList().Count > 1)
        {
          return true;
        }
      }

      return false;
    }
  }
}
