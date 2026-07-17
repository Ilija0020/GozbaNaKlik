using gozba_na_klik_backend.Services.Interfaces;
using gozba_na_klik_backend.Services.Mapping;
using gozba_na_klik_backend.Services;
using gozba_na_klik_backend.Domain.Repositories;
using gozba_na_klik_backend.Infrastructure.PostgreSql;
using gozba_na_klik_backend.Infrastructure.PostgreSql.Repositories;
using gozba_na_klik_backend.Controllers.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace gozba_na_klik_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog(logger);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
            builder.Services.AddScoped<IMealRepository, MealRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IMealService, MealService>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
