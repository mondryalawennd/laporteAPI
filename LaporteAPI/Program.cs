
using LaporteAPI.Persistente.Data;
using LaporteAPI.Persistente.Repository.Interface;
using LaporteAPI.Persistente.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LaporteAPI.Persistente.Service.Interface;
using LaporteAPI.Persistente.Service;
using System.Text;
using LaporteAPI.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LaporteAPI
{
    public class Program
    {
      
        public static void Main(string[] args)
        {
             string CorsPolicy = "_corsPolicy ";


            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("LaporteBD");

            builder.Services.AddControllers().AddJsonOptions(options =>
                                     {
                                         options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                                     });


            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IGenericRepository<Funcionario>, GenericRepository<Funcionario>>();
            builder.Services.AddScoped<IGenericRepository<FuncionarioTelefone>, GenericRepository<FuncionarioTelefone>>();

            builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            builder.Services.AddScoped<IFuncionarioTelefoneRepository, FuncionarioTelefoneRepository>();

            builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
            builder.Services.AddScoped<IFuncionarioTelefoneService, FuncionarioTelefonesService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes("@chave-secret-2@25")),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

            builder.Services.AddCors(dbContextOptions =>
            {
                dbContextOptions.AddPolicy(CorsPolicy,
                   builder =>
                   {
                       builder.WithOrigins("http://localhost:53753/", "http://localhost:53753/")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                   });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "laporte.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Mondrya Lawennd",
                        Email = "lawennd.e@gmail.com",
                    }
                });               
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "laporte.API v1"));
            }

            app.UseCors(builder => builder
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()

                  );
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
