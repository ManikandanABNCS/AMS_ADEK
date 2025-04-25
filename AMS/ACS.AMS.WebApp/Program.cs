//using Telerik.Reporting.Cache.File;
//using Telerik.Reporting.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using ACS.AMS.DAL;
using Microsoft.EntityFrameworkCore;
using ACS.AMS.WebApp;
using ACS.AMS.DAL.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.WebApp.Classes;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using AspNetCore.Unobtrusive.Ajax;

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.TryAddSingleton((Func<IServiceProvider, IReportServiceConfiguration>)(sp =>
    new ReportServiceConfiguration
    {
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
        HostAppId = "WebReportDesignerApp",
        Storage = new FileStorage(),
        ReportSourceResolver = new UriReportSourceResolver(GetReportsDir(sp))
    }));

builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
    HostAppId = "Html5ReportViewerDemo",
    Storage = new FileStorage(),
    ReportSourceResolver = new TypeReportSourceResolver()
            .AddFallbackResolver(new UriReportSourceResolver(System.IO.Path.Combine(GetReportsDir(sp))))
});


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews()
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {

       options.LoginPath = new PathString("/Login/Login/");
       options.AccessDeniedPath = new PathString("/Login/Logout/");
       options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
       options.SlidingExpiration = true;
   });
// Add Kendo UI services to the services container"
builder.Services.AddKendo();
builder.Services.AddUnobtrusiveAjax();

{
    var configuration = builder.Configuration;
    var services = builder.Services;

    LogHelper.EnableTraceLog = configuration.GetValue<bool>("Logging:EnableTraceLog");
    LogHelper.TraceLogLocation = configuration.GetValue<string>("Logging:TraceLogLocation");
    LogHelper.InitLog();

    services
            .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
            // Maintain property names during serialization. See:
            // https://github.com/aspnet/Announcements/issues/194
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

    // Add Kendo UI services to the services container
    services.AddKendo();
    services.AddControllers().AddNewtonsoftJson();
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromDays(1);
    });

    AMSContext.EnableQueryLog = configuration.GetValue<bool>("Logging:EnableQueryLog");
    AMSContext.QueryLogLocation = configuration.GetValue<string>("Logging:QueryLogLocation");
    AMSContext.EnableDBAuditLog = configuration.GetValue<bool>("Logging:EnableDBAuditLog");
    AMSContext.ConnectionString = configuration.GetConnectionString("AMSConnection");

    builder.Services.AddDbContext<AMSContext>(options => options.UseSqlServer(configuration.GetConnectionString("AMSConnection"), b => b.MigrationsAssembly("ACS.AMS.WebApp")));
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    // For Identity
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AMSContext>()
        .AddDefaultTokenProviders();

    // Adding Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });

    services.AddMvc(o =>
    {
        o.AllowEmptyInputInBodyModelBinding = true;
    })
       .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "The field is required.");
    });
    services.AddDistributedMemoryCache();
    services.AddSession();
    services.AddHttpContextAccessor();
   

    //API Related configurations
    //APILogHelper.SetLogFileBaseLocation(configuration["ServiceLogLocation"]);// ("ServiceLogLocation");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.MapRazorPages();
//app.MapDefaultControllerRoute();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization("en-US");


app.UseCookiePolicy(cookiePolicyOptions);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // ... 
});
// Configure the Localization middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

AppHttpContext.Services = app.Services;
SessionUserHelper.CurrentSessionUser = new SessionUserImpl();

//update master grid line items
//try
//{
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "PurchaseOrder");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "SalesOrder");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "GRNWithPO");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "GRNWithoutPO");

//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "TransferOut");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "TransferIn");

//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "BinToBin");

//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "StockAdjustment");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "ItemIssue");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "ItemDispatch");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "Putaway");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "CreditNote");
//    SystemDatabaseHelper.GenerateMasterGridColumns(typeof(TransactionTable), "DebitNote");
//}
//catch(Exception ex)
//{
//    ApplicationErrorLogTable.SaveException(ex);
//}

app.Run();

static string GetReportsDir(IServiceProvider sp)
{
    return Path.Combine(sp.GetService<IWebHostEnvironment>().WebRootPath, "Reports");
}