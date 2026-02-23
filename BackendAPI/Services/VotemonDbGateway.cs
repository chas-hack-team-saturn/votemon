using Microsoft.EntityFrameworkCore;
using BackendAPI.Data;
namespace BackendAPI.Services
{
	public static class VotemonDbGateway
	{
		static public async Task<IResult> ReadDB(VotemonDbContext dB)
		{
			var currentDb = await dB.Pokemons.ToListAsync();

			if (currentDb == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(currentDb);
		}

		static public async Task<IResult> UpdateDB(VotemonDbContext dB)
		{
			for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
			{
				var pokemon = await dB.Pokemons.ElementAtAsync(i + 1);
				pokemon.Name = PokeApiMethods.PokemonNames[i + 1];
			}
			await dB.SaveChangesAsync();

			return VotemonDbGateway.ReadDB(dB).Result;
		}

		static public async Task<IResult> InsertIntoDB(VotemonDbContext dB)
		{
			foreach (var name in PokeApiMethods.PokemonNames)
			{
				var pokemon = new Models.Pokemon();
				pokemon.Name = name;
				await dB.AddAsync(pokemon);
			}
			await dB.SaveChangesAsync();

			return VotemonDbGateway.ReadDB(dB).Result;
		}


		static public void WriteNamesToScriptMySQL()
		{
			// Creates an Update script using MySQL syntax
			using (var writer = new StreamWriter("docker/update.sql", false))
			{
				writer.WriteLine("USE PokeScrandle");

				for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
				{
					writer.WriteLine(
						$"UPDATE Pokemon SET Name = '{PokeApiMethods.PokemonNames[i]}' WHERE DexId = {i + 1};"
						);
				}
			}

			// Creates an INSERT INTO script using MySQL syntax
			using (var writer = new StreamWriter("docker/insert.sql", false))
			{
				writer.WriteLine("USE PokeScrandle");
				writer.WriteLine("INSERT INTO Pokemon (Name) VALUES");

				for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
				{
					if (i == PokeApiMethods.PokemonNames.Count - 1)
						writer.WriteLine($"    ('{PokeApiMethods.PokemonNames[i]}');");
					else
						writer.WriteLine($"    ('{PokeApiMethods.PokemonNames[i]}'),");
				}
			}
		}
	}
}
