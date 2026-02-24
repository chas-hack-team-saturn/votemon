using BackendAPI.Data;
using BackendAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public static class Endpoints
    {
        public static void UseVotemonEndpoints(this WebApplication app)
        {
            // Calls PokeAPIand returns the names of every Pokemon, and
            // then temporarily stores the names in an in-memory list.
            app.MapGet("/names/set", async() =>
            {
                await PokeApiMethods.SetPokemonNames();
                return Results.Ok(PokeApiMethods.PokemonNames);
            });

            // Checks the in-memory list of Pokemon names.
            // If empty, use .MapGet("/admin/names/set") first.
            app.MapGet("/names/get", async() =>
            {
                return Results.Ok(PokeApiMethods.PokemonNames);
            });

            // Checks if Pokemon table in database is empty.
            // If empty, runs insert into
            // If not empty, runs update
            app.MapPut("/names/update", (VotemonDbContext dB) =>
            {
                if (dB.Pokemons.AnyAsync().Result)
                {
                    Console.WriteLine("Database table is not empty");
                    return VotemonDbGateway.UpdateDB(dB);
                }
                else
                {
                    Console.WriteLine("Database table is Empty!!!");
                    return VotemonDbGateway.InsertIntoDB(dB);
                }
            });

            // Returns the contents of the database
            app.MapGet("/get", (VotemonDbContext dB) =>
            {
                return VotemonDbGateway.ReadDbPokemon(dB);
            });


            app.MapGet("/get/top100", async (VotemonDbContext dB) =>
            {
                var results = await dB.Pokemons.OrderByDescending(p => p.Votes).Take(100).ToListAsync();

                if (results == null || results.Count <= 0)
                {
                    Results.NotFound();
                    return results;
                }

                Results.Ok();
                return results;
            });

            // Gets the Pokemon specified with that DexId 
            app.MapGet("/get={id}", async (VotemonDbContext dB, int id) =>
            {
                var pokemon = await dB.Pokemons.FindAsync(id);

                if (id <= 0)
                {
                    Results.NotFound();
                    return pokemon;
                }
                if (pokemon == null)
                {
                    Results.NotFound();
                    return pokemon;
                }

                Results.Ok();
                return pokemon;
            });

            // Adds a vote to the Pokemon with that DexId
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
