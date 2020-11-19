namespace Phoneden.Web
{
  using DataAccess.Context;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Entities;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.HttpOverrides;
  using Phoneden.Services;
  using Phoneden.Services.Interfaces;
  using Services;

  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      string databaseConnectionString = Configuration["Database:DefaultConnectionString"];

      services
        .AddDatabaseDeveloperPageExceptionFilter();

      services
        .AddSingleton<ISmtpConfiguration>(Configuration.GetSection("SmtpConfiguration").Get<SmtpConfiguration>());

      services
        .AddSingleton<IPaginationConfiguration>(Configuration.GetSection("PaginationConfiguration").Get<PaginationConfiguration>());

      services
        .AddEntityFrameworkNpgsql()
        .AddDbContext<PdContext>(options => options.UseNpgsql(databaseConnectionString));

      services
        .AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<PdContext>()
        .AddDefaultTokenProviders();

      services
        .Configure<CookiePolicyOptions>(options =>
        {
          options.CheckConsentNeeded = context => true;
          options.MinimumSameSitePolicy = SameSiteMode.None;
        });

      services
        .AddScoped<IEmailSender, EmailSender>();

      services
        .AddScoped<IAddressService, AddressService>();

      services
        .AddScoped<IBrandService, BrandService>();

      services
        .AddScoped<ICategoryService, CategoryService>();

      services
        .AddScoped<IContactService, ContactService>();

      services
        .AddScoped<ICustomerService, CustomerService>();

      services
        .AddScoped<IExpenseService, ExpenseService>();

      services
        .AddScoped<IPurchaseOrderInvoiceService, PurchaseOrderInvoiceService>();

      services
        .AddScoped<IPaymentService, PaymentService>();

      services
        .AddScoped<IProductService, ProductService>();

      services
        .AddScoped<IPurchaseOrderService, PurchaseOrderService>();

      services
        .AddScoped<IQualityService, QualityService>();

      services
        .AddScoped<IReportService, ReportService>();

      services
        .AddScoped<ISaleOrderService, SaleOrderService>();

      services
        .AddScoped<ISaleOrderInvoiceService, SaleOrderInvoiceService>();

      services
        .AddScoped<ISaleOrderInvoiceService, SaleOrderInvoiceService>();

      services
        .AddScoped<ISaleOrderPaymentService, SaleOrderPaymentService>();

      services
        .AddScoped<ISupplierService, SupplierService>();

      services
        .AddScoped<IReturnService, ReturnService>();

      services
        .AddTransient<INotificationService, NotificationService>();

      services
        .AddTransient<IEmailBuilderService, EmailBuilderService>();

      services
        .AddControllersWithViews();

      services
        .AddCloudscribePagination();
    }

    public void Configure(
      IApplicationBuilder app,
      IWebHostEnvironment env,
      PdContext context)
    {
      if (env.IsDevelopment())
      {
        app
          .UseDeveloperExceptionPage();
      }
      else
      {
        app
          .UseExceptionHandler("/Home/Error");

        app
          .UseHsts();
      }

      app
        .UseHttpsRedirection();

      app
        .UseStaticFiles();

      app
        .UseCookiePolicy();

      app
        .UseForwardedHeaders(new ForwardedHeadersOptions
        {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

      app
        .UseRouting();

      app
        .UseAuthentication();

      app
        .UseAuthorization();

      app
        .UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });
    }
  }
}
