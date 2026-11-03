using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PmsRepositories.Repositories.HttpClientHelper;
using UBSProptRepository.Repository;
using AIServices;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register environment variable for Firebase
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
    @"/Users/mapmac/DotNetCore/FabPropertiesWeb/landlorddotnetcore-firebase-adminsdk-rb24w-f082b1293e.json");

// ✅ CORS configuration
builder.Services.AddCors(builder =>
{
    builder.AddPolicy("AllowAll", options =>
    {
        options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add an environment var val 
builder.Services.Configure<UBSAuthApiConfig>(
    builder.Configuration.GetSection("UBSAuthApiConfig")
    
);
builder.Services.Configure<UBSTenantApiConfig>(
    builder.Configuration.GetSection("UBSTenantApiConfig")
);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblyContaining<EmailValidator>();// register validators
builder.Services.AddValidatorsFromAssemblyContaining<PasswordValidator>();// register validators
builder.Services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior
builder.Services.AddFluentValidationClientsideAdapters();

// Calling AI Service
builder.Services.AddAIService(builder.Configuration);

// Register HttpClientService to the DI container
builder.Services.AddHttpClient<HttpClientService>();


builder.Services.AddScoped<CommonDropdownRepository, CommonDropdownRepository>();
builder.Services.AddScoped<UBSAuthRepository, UBSAuthRepository>();
builder.Services.AddScoped<NotificationRepo, NotificationRepo>();
builder.Services.AddScoped<RenewalFlowRepo, RenewalFlowRepo>();
builder.Services.AddScoped<ContractRepository, ContractRepository>();
builder.Services.AddScoped<FaqsRepository, FaqsRepository>(); 
builder.Services.AddScoped<ProfileRepository, ProfileRepository>();
builder.Services.AddScoped<OffersRepository, OffersRepository>(); 
builder.Services.AddScoped<PaymentsRepository, PaymentsRepository>(); 
builder.Services.AddScoped<ServiceRequestRepository, ServiceRequestRepository>();
builder.Services.AddScoped<DashboardRepo, DashboardRepo>();
builder.Services.AddScoped<UBSAuthApiConfig>();       // 🔄 If it depends on IConfiguration, it's now injected
builder.Services.AddScoped<UBSApiHelper>();


// Register other services and controllers
builder.Services.AddControllers();

// Session & Cookies
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    //options.LoginPath = "/Auth/AuthScreen"; *
    //options.LogoutPath = "/Account/Logout"; // Optional: where to go on logout
    //options.AccessDeniedPath = "/Account/AccessDenied"; // Optional: for [Authorize(Roles = ...)]
});

// Configure other necessary services like logging, configuration, etc.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // ✅ Needed for cookie authentication
app.UseAuthorization();

// Add custom middleware to set language based on cookies
app.Use(async (context, next) =>
{
    // Retrieve the saved language from cookies
    var savedLanguage = context.Request.Cookies["selectedLanguage"];
    if (!string.IsNullOrEmpty(savedLanguage))
    {
        // Set the language in LabelsLandlordPortal class
        AuthLabelsPortal.SetLanguage(savedLanguage);
    }
    else
    {
        AuthLabelsPortal.SetLanguage("English");
    }
    await next.Invoke();
});



app.UseEndpoints(endpoints =>
{
    // Area route
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Auth}/{action=AuthScreen}/{id?}"
    );

    // Default route - explicitly specifying area
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=AuthScreen}/{id?}",
        defaults: new { area = "Users" }
    );
});

app.Run();
