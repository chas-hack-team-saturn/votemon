using BackendAPI.Data;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Services;

namespace BackendAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// This array contains URLs that allows Cross-Oirigin-Resource-Sharing.
			string[] urlCors = ["https://votemon.pabu.dev/"];

			var password = Environment.GetEnvironmentVariable("MARIADB_PASSWORD") ?? "asdf";
			var server = Environment.GetEnvironmentVariable("MARIADB_HOST") ?? "localhost";
			var port = Environment.GetEnvironmentVariable("MARIADB_PORT") ?? "9999";
			string connectionString = $"Server={server};Port={port};Database=Votemon;Uid=root;Pwd={password};";
			var serverVersion = new MariaDbServerVersion(new Version(12, 1, 2));


			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAuthorization();

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			// Creates new Cross-Origin-Resource-Sharing policy
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("Votemon", (policy) =>
				{
					policy.WithOrigins(urlCors).AllowAnyMethod().AllowAnyHeader();
				});
			});

			builder.Services.AddDbContext<VotemonDbContext>(options => options.UseMySql(connectionString, serverVersion, options => options.UseMicrosoftJson()));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			// Use Cross-Origin-Resource-Sharing policy.
			app.UseCors("Votemon");

			app.UseHttpsRedirection();

			app.UseAuthorization();

			// Enables our API endpoints.
			app.UseVotemonEndpoints();

			app.Run();
		}
	}
}
