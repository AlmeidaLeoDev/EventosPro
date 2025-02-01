using EventosPro.Context;
using EventosPro.Repositories.Implementations;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Implementations;
using EventosPro.Services.Interfaces;
using EventosPro.Validators.EventViewModelsValidators;
using EventosPro.Validators.UserViewModelsValidators;
using EventosPro.ViewModels.Events;
using EventosPro.ViewModels.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContex>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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


var app = builder.Build();

// Quartz
builder.Services.AddQuartz(q =>
{
    var cleanupJobKey = new JobKey("DatabaseCleanupJob");
    q.AddJob<DatabaseCleanupJob>(opts => opts.WithIdentity(cleanupJobKey));
    q.AddTrigger(opts => opts
        .ForJob(cleanupJobKey)
        .WithIdentity("DatabaseCleanupTrigger")
        .WithCronSchedule("0 */30 * ? * *")); // A cada 30 minutos

    q.UseSimpleTypeLoader();
    q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 10);
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.AwaitApplicationStarted = true;
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
