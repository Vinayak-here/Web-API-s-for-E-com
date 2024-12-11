using E_Com.Data;
using E_Com.DTO_s.RequestDTO_s;
using E_Com.Repository.IRepository;
using E_Com.Repository;
using E_Com.Service.IService;
using E_Com.Service;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Register FluentValidation validators
        builder.Services.AddValidatorsFromAssemblyContaining<RegistrationRequestDTO>();

        // Configure DbContext for PostgreSQL
        builder.Services.AddDbContext<EComDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
        );

        // Register other services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISellerRepository, SellerRepository>();
        builder.Services.AddScoped<ISellerService, SellerService>();

        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer(); // Add endpoint explorer to the DI container
        builder.Services.AddSwaggerGen(); // Add Swagger generator service to DI container

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enable Swagger middleware
            app.UseSwaggerUI(); // Enable Swagger UI for testing
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
