namespace Phoneden.Services
{
  using ViewModels;

  public interface IAddressService
  {
    AddressViewModel GetAddress(int id, bool isSupplierAddress);

    void AddAddress(AddressViewModel addressVm);

    void UpdateAddress(AddressViewModel addressVm);

    void DeleteAddress(int id, bool isSupplierAddress);

    bool CanAddressBeDeleted(int id, bool isSupplierAddress);
  }
}
