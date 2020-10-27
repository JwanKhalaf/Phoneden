namespace Phoneden.Services
{
  using ViewModels;

  public interface IContactService
  {
    ContactViewModel GetContact(int id, bool isSupplierContact);

    void AddContact(ContactViewModel contactVm);

    void UpdateContact(ContactViewModel contactVm);

    void DeleteContact(int id, bool isSupplierContact);

    bool CanContactBeDeleted(int id, bool isSupplierContact);
  }
}
