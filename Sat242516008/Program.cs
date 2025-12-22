using DbContexts;
using Microsoft.EntityFrameworkCore;
using Providers;
using Sat242516008.Components;
using UnitOfWorks;
using Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options; // Eklendi

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Services.ReportService>();

// 1. Veritabaný Baðlantý Ayarý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<MyDbModel_DbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. UnitOfWork ve Provider Kayýtlarý
builder.Services.AddScoped<IMyDbModel_UnitOfWork, MyDbModel_UnitOfWork<MyDbModel_DbContext>>();
builder.Services.AddScoped<IMyDbModel_Provider, MyDbModel_Provider>();

// --- MADDE 20: DÝL DESTEÐÝ SERVÝS KAYITLARI ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Dil Seçeneklerini Yapýlandýr
var supportedCultures = new[] { "tr-TR", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;

    // ÖNEMLÝ: Dilin çerezden (cookie) okunmasýný en öncelikli hale getiriyoruz
    options.RequestCultureProviders.Clear(); // Varsayýlanlarý temizle
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider()); // 1. sýrada çereze bak
    options.RequestCultureProviders.Insert(1, new QueryStringRequestCultureProvider()); // 2. sýrada URL'e bak
});

// 3. Blazor ve Güvenlik Servisleri
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AuthService>();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// --- MADDE 20: DÝL DESTEÐÝ ARA KATMANINI AKTÝF ET ---
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

// 4. Middleware (Ara Katman) Yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();