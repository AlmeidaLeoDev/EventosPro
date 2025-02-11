using EventosPro.Context;
using EventosPro.Mapping;
using EventosPro.Middlewares;
using EventosPro.Repositories.Implementations;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Implementations;
using EventosPro.Services.Interfaces;
using EventosPro.Validators.EventViewModelsValidators;
using EventosPro.Validators.UserViewModelsValidators;
using EventosPro.ViewModels.Events;
using EventosPro.ViewModels.Users;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using EventosPro.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://0.0.0.0:7247");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContex>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICookieService, CookieService>();

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventInviteRepository, EventInviteRepository>();

// Services
builder.Services.AddSingleton<ICryptographyService, CryptographyService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDatabaseCleanupService, DatabaseCleanupService>();
builder.Services.AddScoped<IEventService, EventService>();

// Validators
builder.Services.AddScoped<IValidator<ChangePasswordViewModel>, ChangePasswordValidator>();
builder.Services.AddScoped<IValidator<EmailConfirmationViewModel>, EmailConfirmationValidator>();
builder.Services.AddScoped<IValidator<ForgotPasswordViewModel>, ForgotPasswordValidator>();
builder.Services.AddScoped<IValidator<LoginViewModel>, LoginValidator>();
builder.Services.AddScoped<IValidator<RegisterViewModel>, RegisterValidator>();
builder.Services.AddScoped<IValidator<ResetPasswordViewModel>, ResetPasswordValidator>();
builder.Services.AddScoped<IValidator<UserProfileViewModel>, UserProfileValidator>();

builder.Services.AddScoped<IValidator<CreateEventViewModel>, CreateEventValidator>();
builder.Services.AddScoped<IValidator<CreateEventInviteViewModel>, CreateEventInviteValidator>();
builder.Services.AddScoped<IValidator<EventDetailsViewModel>, EventDetailsValidator>();
builder.Services.AddScoped<IValidator<EventInviteViewModel>, EventInviteValidator>();
builder.Services.AddScoped<IValidator<EventListViewModel>, EventListValidator>();
builder.Services.AddScoped<IValidator<RespondToInviteViewModel>, RespondToInviteValidator>();
builder.Services.AddScoped<IValidator<UpdateEventViewModel>, UpdateEventValidator>();

// Email
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<EmailSettings>>().Value);

// Mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Quartz
builder.Services.AddQuartz(q =>
{
    var cleanupJobKey = new JobKey("DatabaseCleanupJob");
    q.AddJob<DatabaseCleanupJob>(opts => opts.WithIdentity(cleanupJobKey));
    q.AddTrigger(opts => opts
        .ForJob(cleanupJobKey)
        .WithIdentity("DatabaseCleanupTrigger")
        .WithCronSchedule("0 0/30 * * * ?")); // A cada 30 minutos

    q.UseSimpleTypeLoader();
    q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 10);
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.AwaitApplicationStarted = true;
});

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("SecurePolicy",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://9a12-2804-56c-a404-db00-c1bb-e456-128f-3239.ngrok-free.app",
                    "https://*.ngrok-free.app"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition")
                .SetIsOriginAllowed(origin => true); // Permite qualquer origem durante desenvolvimento
        });
});

// Hsts
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

// Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    options.Cookie.Domain = ".ngrok-free.app";
});

// --
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("SecurePolicy");

// Middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
