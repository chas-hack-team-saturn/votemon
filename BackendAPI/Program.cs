using BackendAPI.Data;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace BackendAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var password = Environment.GetEnvironmentVariable("MARIADB_PASSWORD");
            string connectionString = $"Server=db;Port=3306;Database=PokeScrandle;Uid=root;Pwd={password};";
            var serverVersion = new MariaDbServerVersion(new Version(12, 1, 2));


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            Console.WriteLine(connectionString);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            builder.Services.AddDbContext<PokeScrandleDbContext>(options => options.UseMySql(connectionString, serverVersion, options => options.UseMicrosoftJson()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();





            app.MapGet("/get", async (PokeScrandleDbContext dB) =>
            {
                var results = await dB.Pokemons.ToListAsync();

                if (results.Count < 1 || results == null)
                {
                    Results.NotFound();
                    return results;
                }

                Results.Ok(results);
                return results;
            });


            app.MapGet("/get/top100", async (PokeScrandleDbContext dB) =>
            {
                var results = await dB.Pokemons.OrderByDescending(p => p.Votes).Take(100).ToListAsync();

                if (results == null || results.Count < 1)
                {
                    Results.NotFound();
                    return results;
                }

                Results.Ok();
                return results;
            });



            app.MapGet("/get={id}", async (PokeScrandleDbContext dB, int id) =>
            {
                var pokemon = await dB.Pokemons.FindAsync(id);

                if (pokemon == null)
                {
                    Results.NotFound();
                    return pokemon;
                }

                Results.Ok();
                return pokemon;
            });





            app.MapPut("/vote={id}", async (PokeScrandleDbContext dB, int id) =>
            {
                var pokemon = await dB.Pokemons.FindAsync(id);
                if (pokemon == null)
                {
                    Results.NotFound();
                    return;
                }

                var oldVotes = pokemon!.Votes;

                int? newVotes = oldVotes + 1;

                pokemon!.Votes = newVotes;

                await dB.SaveChangesAsync();
            });

            app.Run();
        }
    }
}
