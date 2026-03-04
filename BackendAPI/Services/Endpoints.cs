using BackendAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
	public static class Endpoints
	{
		public static void UseVotemonEndpoints(this WebApplication app)
		{
			// Returns every Pokemons in the database.
			app.MapGet("/get", (VotemonDbContext dB) =>
			{
				return VotemonDbGateway.ReadDBPokemon(dB);
			});

			// Returns the Pokemon specified by the inputed DexId.
			app.MapGet("/get={dexId}", (VotemonDbContext dB, int dexId) =>
			{
				return VotemonDbGateway.ReadDBPokemon(dB, dexId);
			});

			// Returns the top 100 Pokemons with the highest ELO rating.
			app.MapGet("/get/top100", (VotemonDbContext dB) =>
			{
				return VotemonDbGateway.ReadDBPokemonELORating(dB);
			});

			// Returns the top {number} of Pokemons with the highest ELO rating after {skipped} amount.
			app.MapGet("/get/top={number}&skip={skipped}", (VotemonDbContext dB, int number, int skipped) =>
			{
				return VotemonDbGateway.ReadDBPokemonELORating(dB, skipped, number);
			});

			// Does some ELO magic. Not sure how you'll get winner or loser DexId though.
			//
			//   Try this syntax: "/battle?winnerDexId={int}&loserDexId{int}/"
			//
			app.MapPut("/battle", (VotemonDbContext dB, int winnerDexId, int loserDexId) =>
			{
				return VotemonDbGateway.UpdateDBPokemonBattle(dB, winnerDexId, loserDexId);
			});

			// Adds a vote to the Pokemon with that DexId
			//   If we can get the ELO system working, "/battle" instead. 
			app.MapPut("/vote={dexId}", async (VotemonDbContext dB, int dexId) =>
			{
				return VotemonDbGateway.UpdateDBPokemonVote(dB, dexId);
			});





			#region Used for testing, ignore these

			// Calls PokeAPI to get and store the names of every Pokemon on an in-memory list.
			app.MapGet("/names/set", async () =>
			{
				await PokeApiMethods.SetPokemonNames();
				return Results.Ok(PokeApiMethods.PokemonNames);
			});

			// Checks the in-memory list of Pokemon names.
			//   If empty, use .MapGet("/admin/names/set") first.
			app.MapGet("/names/get", async () =>
			{
				return Results.Ok(PokeApiMethods.PokemonNames);
			});

			// Checks if Pokemon table in database is empty.
			//   If empty, runs insert into.
			//   If not empty, runs update.
			app.MapPut("/names/update", (VotemonDbContext dB) =>
			{
				if (dB.Pokemons.AnyAsync().Result)
				{
					Console.WriteLine("Database table is not empty. Running UpdateDBPokemonNames().");
					return VotemonDbGateway.UpdateDBPokemonNames(dB);
				}
				else
				{
					Console.WriteLine("Database table is empty. Running InsertDBPokemonNames().");
					return VotemonDbGateway.InsertDBPokemonNames(dB);
				}
			});

			#endregion
		}
	}
}
