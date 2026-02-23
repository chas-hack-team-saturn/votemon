using BackendAPI.Data;
using BackendAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public static class Endpoints
    {
        public static void UseVotemonEndpoints(this WebApplication app)
        {
            // Calls PokeAPIand returns name of Pokemons, and store then in memory
            app.MapGet("/admin/names/set", async() =>
            {
                await PokeApiMethods.SetPokemonNames();
                return Results.Ok(PokeApiMethods.PokemonNames);
            });

            // Checks Pokemon names stored in memory
            app.MapGet("/admin/names/get", async() =>
            {
                return Results.Ok(PokeApiMethods.PokemonNames);
            });

            // Inserts Pokemon names into database
            app.MapPost("/admin/names/insert", (VotemonDbContext dB) =>
            {
                if (dB.Pokemons.AnyAsync().Result)
                {
                    Console.WriteLine("Is not empty");
                    return VotemonDbGateway.UpdateDB(dB);
                }
                else
                {
                    Console.WriteLine("Is Empty!!!");
                    return VotemonDbGateway.InsertIntoDB(dB);
                }
            });




            // Returns the contents of the database
            app.MapGet("pokemons/get", (VotemonDbContext dB) =>
            {
                return VotemonDbGateway.ReadDB(dB);
            });





            app.MapGet("/get", async (VotemonDbContext dB) =>
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


            app.MapGet("/get/top100", async (VotemonDbContext dB) =>
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



            app.MapGet("/get={id}", async (VotemonDbContext dB, int id) =>
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





            app.MapPut("/vote={id}", async (VotemonDbContext dB, int id) =>
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
        }




    }
}
