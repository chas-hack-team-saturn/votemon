using BackendAPI.Data;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using BackendAPI.Services;

namespace BackendAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var password = Environment.GetEnvironmentVariable("MARIADB_PASSWORD") ?? "asdf";
            var server = Environment.GetEnvironmentVariable("MARIADB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("MARIADB_PORT") ?? "9999";
            string connectionString = $"Server={server};Port={port};Database=PokeScrandle;Uid=root;Pwd={password};";
            var serverVersion = new MariaDbServerVersion(new Version(12, 1, 2));


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            Console.WriteLine(connectionString);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            builder.Services.AddDbContext<VotemonDbContext>(options => options.UseMySql(connectionString, serverVersion, options => options.UseMicrosoftJson()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseVotemonEndpoints();

            app.Run();
        }
    }
}
