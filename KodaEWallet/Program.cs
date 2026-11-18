using KodaEWallet.Data;
using KodaEWallet.Exceptions;
using KodaEWallet.Repositories;
using KodaEWallet.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")
		?? throw new InvalidOperationException("ConnectionStrings:Default belum diisi")));
		
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IAuditService, AuditService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	using (IServiceScope scope = app.Services.CreateScope())
	{
		await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
	}
	await DbSeeder.SeedAsync(app.Services);
}

app.UseExceptionHandler();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Koda E-Wallet API";
});
app.UseHttpsRedirection();
app.MapControllers();

app.Run();