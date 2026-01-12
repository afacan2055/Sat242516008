using DbContexts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Providers;
using Sat242516008.Components;
using Sat242516008.Components.Account;
using Sat242516008.Data;
using UnitOfWorks;

using QuestPDF.Infrastructure; // Bunu en üste ekle

// ...
// var builder = WebApplication.CreateBuilder(args); satýrýndan ÖNCE:

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<Sat242516008.Services.ReportService>();

// --- LOGGERS SERVÝSLERÝ ---
builder.Services.AddScoped<Sat242516008.Loggers.FileLogger>();
builder.Services.AddScoped<Sat242516008.Loggers.DbLogger>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1. STANDART IDENTITY DATABASE BAÐLANTISI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- [EKLENEN KISIM] HOCANIN MÝMARÝSÝ ÝÇÝN GEREKLÝ SERVÝSLER ---
// Bu blok olmadan "Provider not registered" hatasý alýrsýn.

// 2. Custom DbContext Baðlantýsý (Hocanýn yapýsý)
builder.Services.AddDbContext<MyDbModel_DbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. UnitOfWork Servisi
builder.Services.AddScoped<IMyDbModel_UnitOfWork, MyDbModel_UnitOfWork<MyDbModel_DbContext>>();

// 4. Provider Servisi (Products sayfasýnda @inject edilen servis bu)
builder.Services.AddScoped<IMyDbModel_Provider, MyDbModel_Provider>();

// ---------------------------------------------------------------

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// --- ROL VE KULLANICI EKLEME (SEEDING) BAÞLANGICI ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. Rolleri Oluþtur
    var roles = new[] { "Admin", "Personel", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // 2. Admin Kullanýcýsýný Oluþtur
    var adminEmail = "admin@proje.com";
    var adminPassword = "Sau.123456*";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
// --- ROL VE KULLANICI EKLEME SONU ---

app.Run();