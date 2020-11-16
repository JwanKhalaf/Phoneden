namespace Phoneden.DataAccess.Initialisations
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Context;
  using Entities;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Logging;

  public class DbInitializer
  {
    public static async Task Initialize(PdContext context,
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager,
      ILogger<DbInitializer> logger)
    {
      Console.WriteLine("Attempting database initialisation.");

      context.Database.Migrate();

      if (context.Users.Any())
      {
        return; // DB has been seeded
      }

      await CreateApplicationUsersWithRoles(userManager, roleManager, logger);
      await CreateTestSuppliersAsync(context, logger);
      await CreateTestCustomersAsync(context, logger);
      await CreateTestBrandsAsync(context, logger);
      await CreateTestCategoriesAsync(context, logger);
      await CreateTestQualitiesAsync(context, logger);
      await CreateTestProductsAsync(context, logger);
    }

    private static async Task CreateTestProductsAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      logger.LogInformation("Adding some products for test purposes");
      Product iphonex = new Product
      {
        Barcode = "123456789",
        Name = "iPhone X",
        Description = "This is the Apple iPhone X",
        Colour = Colour.Black,
        UnitCostPrice = 0,
        UnitSellingPrice = 0,
        AlertThreshold = 2,
        QualityId = 1,
        CategoryId = 1,
        BrandId = 1,
        Quantity = 0,
      };

      Product galaxysnine = new Product
      {
        Barcode = "987654321",
        Name = "Galaxy S9",
        Description = "This is the Samsung Galaxy S9",
        Colour = Colour.Black,
        UnitCostPrice = 0,
        UnitSellingPrice = 0,
        AlertThreshold = 2,
        QualityId = 1,
        CategoryId = 1,
        BrandId = 2,
        Quantity = 0,
      };

      Product spigeniPhoneCase = new Product
      {
        Barcode = "12345",
        Name = "iPhone SE (2020) Tough Armor Case",
        Description = "Genuine iPhone SE (2020) Tough Armor Spigen Case",
        Colour = Colour.Black,
        UnitCostPrice = 5,
        UnitSellingPrice = 8,
        AlertThreshold = 10,
        QualityId = 1,
        CategoryId = 2,
        BrandId = 3,
        Quantity = 50,
      };

      context.Products.Add(iphonex);
      context.Products.Add(galaxysnine);
      context.Products.Add(spigeniPhoneCase);

      await context.SaveChangesAsync();
    }

    private static async Task CreateTestQualitiesAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      logger.LogInformation("Adding original and fake qualities for test purposes");
      Quality original = new Quality { Name = "Original" };
      Quality fake = new Quality { Name = "Fake" };
      context.Qualities.Add(original);
      context.Qualities.Add(fake);
      await context.SaveChangesAsync();
    }

    private static async Task CreateTestCategoriesAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      logger.LogInformation("Adding handsets and cases categories for test purposes");
      Category handsets = new Category { Name = "Handsets" };
      Category cases = new Category { Name = "Cases" };
      context.Categories.Add(handsets);
      context.Categories.Add(cases);
      await context.SaveChangesAsync();
    }

    private static async Task CreateTestBrandsAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      logger.LogInformation("Adding apple, samsung and spigen brands for test purposes");
      Brand apple = new Brand { Name = "Apple" };
      Brand samsung = new Brand { Name = "Samsung" };
      Brand spigen = new Brand { Name = "Spigen" };

      context.Brands.Add(apple);
      context.Brands.Add(samsung);
      context.Brands.Add(spigen);

      await context.SaveChangesAsync();
    }

    private static async Task CreateTestSuppliersAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      Supplier supplierx = CreateSupplier("Supplier X", "one@example.com", "www.supplier-x.com", "John", "Doe", logger);
      Supplier suppliery = CreateSupplier("Supplier Y", "two@example.com", "www.supplier-y.com", "James", "Brown", logger);
      Supplier supplierz = CreateSupplier("Supplier Z", "three@example.com", "www.supplier-z.com", "Adam", "Smith", logger);
      context.Suppliers.Add(supplierx);
      context.Suppliers.Add(suppliery);
      context.Suppliers.Add(supplierz);
      await context.SaveChangesAsync();
    }

    private static async Task CreateTestCustomersAsync(PdContext context, ILogger<DbInitializer> logger)
    {
      Customer customerx = CreateCustomer("Customer X", "one@example.com", "www.customer-x.com", "Mark", "Watson", logger);
      Customer customery = CreateCustomer("Customer Y", "two@example.com", "www.customer-y.com", "Nick", "Payne", logger);
      Customer customerz = CreateCustomer("Customer Z", "three@example.com", "www.customer-z.com", "Chis", "Poe", logger);
      context.Customers.Add(customerx);
      context.Customers.Add(customery);
      context.Customers.Add(customerz);
      await context.SaveChangesAsync();
    }

    private static Supplier CreateSupplier(string name, string email, string website, string contactFirstName, string contactLastName, ILogger<DbInitializer> logger)
    {
      logger.LogInformation($"Adding {name} Supplier for test purposes");
      Supplier supplier = new Supplier
      {
        Name = name,
        Email = email,
        CreatedOn = DateTime.UtcNow,
        Description = "This is just a test Supplier",
        IsDeleted = false,
        Phone = "0123456789",
        Website = website
      };

      AssignAddressToSupplier(name, supplier);

      AssignContactToSupplier(contactFirstName, contactLastName, supplier);

      return supplier;
    }

    private static Customer CreateCustomer(string name, string email, string website, string contactFirstName, string contactLastName, ILogger<DbInitializer> logger)
    {
      logger.LogInformation($"Adding {name} Customer for test purposes");

      Customer customer = new Customer
      {
        Name = name,
        Email = email,
        CreatedOn = DateTime.UtcNow,
        Description = "This is just a test Supplier",
        IsDeleted = false,
        Phone = "0123456789",
        Website = website,
        Code = "CUS001"
      };

      AssignAddressToCustomer(name, customer);

      AssignContactToCustomer(contactFirstName, contactLastName, customer);

      return customer;
    }

    private static void AssignContactToSupplier(string firstName, string lastName, Supplier supplier)
    {
      supplier.Contacts = new List<SupplierContact>();

      SupplierContact supplierContact = new SupplierContact
      {
        Title = "Mr",
        FirstName = firstName,
        LastName = lastName,
        Department = "Sales",
        Email = firstName.ToLower() + "." + lastName.ToLower() + "@example.com",
        Phone = "0123456789"
      };

      supplier.Contacts.Add(supplierContact);
    }

    private static void AssignContactToCustomer(string firstName, string lastName, Customer customer)
    {
      customer.Contacts = new List<CustomerContact>();

      CustomerContact customerContact = new CustomerContact
      {
        Title = "Mr",
        FirstName = firstName,
        LastName = lastName,
        Department = "Sales",
        Email = firstName.ToLower() + "." + lastName.ToLower() + "@example.com",
        Phone = "0123456789"
      };

      customer.Contacts.Add(customerContact);
    }


    private static void AssignAddressToSupplier(string identifier, Supplier supplier)
    {
      supplier.Addresses = new List<SupplierAddress>();

      SupplierAddress supplierAddress = new SupplierAddress
      {
        AddressLine1 = identifier + " Some Road",
        AddressLine2 = identifier + " Something",
        Area = "Area " + identifier,
        City = "City " + identifier,
        County = "County " + identifier,
        Country = "Country " + identifier,
        PostCode = "HD3 9LK"
      };

      supplier.Addresses.Add(supplierAddress);
    }

    private static void AssignAddressToCustomer(string identifier, Customer customer)
    {
      customer.Addresses = new List<CustomerAddress>();

      CustomerAddress customerAddress = new CustomerAddress
      {
        AddressLine1 = identifier + " Some Road",
        AddressLine2 = identifier + " Something",
        Area = "Area " + identifier,
        City = "City " + identifier,
        County = "County " + identifier,
        Country = "Country " + identifier,
        PostCode = "HD3 9LK"
      };

      customer.Addresses.Add(customerAddress);
    }

    private static async Task CreateApplicationUsersWithRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
    {
      const string administratorRole = "Admin";
      const string clerkRole = "Clerk";

      await CreateRole(roleManager, logger, administratorRole);
      await CreateRole(roleManager, logger, clerkRole);

      const string wahabEmail = "w.hamaamin@phoneden.co.uk";
      const string wahabPassword = "=FEf^K]JHRi.>8tB2k.MTgPD~@DnKcgJi7BPU@9D";
      const string wahabDisplayName = "Ari";

      ApplicationUser wahabUser = await CreateDefaultUser(userManager, logger, wahabDisplayName, wahabEmail);
      await SetPasswordForUser(userManager, logger, wahabEmail, wahabUser, wahabPassword);
      await AssignRoleToUser(userManager, logger, wahabEmail, administratorRole, wahabUser);

      const string ramadanEmail = "r.ali@phoneden.co.uk";
      const string ramadanPassword = "BkG-N3n%=F.dLU^L.dimewXM,gTDgtsoaPck=3Wq";
      const string ramadanDisplayName = "Ramadan";

      ApplicationUser ramadanUser = await CreateDefaultUser(userManager, logger, ramadanDisplayName, ramadanEmail);
      await SetPasswordForUser(userManager, logger, ramadanEmail, ramadanUser, ramadanPassword);
      await AssignRoleToUser(userManager, logger, ramadanEmail, clerkRole, ramadanUser);

      const string mariwanEmail = "m.hassan@phoneden.co.uk";
      const string mariwanPassword = ")rurR8}dw~Pq:z=pCtf3^>E_hHY_X4gFCHG4w)p:";
      const string mariwanDisplayName = "Mariwan";

      ApplicationUser mariwanUser = await CreateDefaultUser(userManager, logger, mariwanDisplayName, mariwanEmail);
      await SetPasswordForUser(userManager, logger, mariwanEmail, mariwanUser, mariwanPassword);
      await AssignRoleToUser(userManager, logger, mariwanEmail, clerkRole, mariwanUser);

      const string ahmedEmail = "a.karim@phoneden.co.uk";
      const string ahmedPassword = "FWeAg93P?h,fUiM7wtB@f]dD64oiCubtti=rGBq4";
      const string ahmedDisplayName = "Ahmed";

      ApplicationUser ahmedUser = await CreateDefaultUser(userManager, logger, ahmedDisplayName, ahmedEmail);
      await SetPasswordForUser(userManager, logger, ahmedEmail, ahmedUser, ahmedPassword);
      await AssignRoleToUser(userManager, logger, ahmedEmail, clerkRole, ahmedUser);
    }

    private static async Task CreateRole(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger, string role)
    {
      logger.LogInformation($"Create the role `{role}` for application");
      IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
      if (result.Succeeded)
      {
        logger.LogDebug($"Created the role `{role}` successfully");
      }
      else
      {
        ApplicationException exception = new ApplicationException($"Default role `{role}` cannot be created");
        logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(result));
        throw exception;
      }
    }

    private static async Task<ApplicationUser> CreateDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string displayName, string email)
    {
      logger.LogInformation($"Create default user with email `{email}` for application");

      ApplicationUser user = new ApplicationUser
      {
        DisplayUsername = displayName,
        Email = email,
        UserName = email
      };

      IdentityResult identityResult = await userManager.CreateAsync(user);

      if (identityResult.Succeeded)
      {
        logger.LogDebug($"Created default user `{email}` successfully");
      }
      else
      {
        ApplicationException exception = new ApplicationException($"Default user `{email}` cannot be created");
        logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(identityResult));
        throw exception;
      }

      ApplicationUser createdUser = await userManager.FindByEmailAsync(email);
      return createdUser;
    }

    private static async Task SetPasswordForUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, ApplicationUser user, string password)
    {
      logger.LogInformation($"Set password for default user `{email}`");
      IdentityResult identityResult = await userManager.AddPasswordAsync(user, password);
      if (identityResult.Succeeded)
      {
        logger.LogTrace($"Set password `{password}` for default user `{email}` successfully");
      }
      else
      {
        ApplicationException exception = new ApplicationException($"Password for the user `{email}` cannot be set");
        logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(identityResult));
        throw exception;
      }
    }

    private static async Task AssignRoleToUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, string role, ApplicationUser user)
    {
      logger.LogInformation($"Add default user `{email}` to role '{role}'");
      IdentityResult identityResult = await userManager.AddToRoleAsync(user, role);
      if (identityResult.Succeeded)
      {
        logger.LogDebug($"Added the role '{role}' to default user `{email}` successfully");
      }
      else
      {
        ApplicationException exception = new ApplicationException($"The role `{role}` cannot be set for the user `{email}`");
        logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(identityResult));
        throw exception;
      }
    }

    private static string GetIdentiryErrorsInCommaSeperatedList(IdentityResult ir)
    {
      string errors = null;
      foreach (var identityError in ir.Errors)
      {
        errors += identityError.Description;
        errors += ", ";
      }
      return errors;
    }
  }
}
