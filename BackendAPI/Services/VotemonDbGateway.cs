using Microsoft.EntityFrameworkCore;
using BackendAPI.Data;

namespace BackendAPI.Services
{
	public static class VotemonDbGateway
	{
        /// <summary>
        ///		Returns the contents of the Pokemon table in database. 
        /// </summary>
        /// <param name="dB">The database to read from.</param>
        /// <returns>
        ///		<list type="bullet">
        ///			<item>200 OK status alongside all records in Pokemon table.</item>
		///			<item>Otherwise if empty, 404 NOT FOUND status.</item>
        ///		</list>
        /// </returns>
        static public async Task<IResult> ReadDbPokemon(VotemonDbContext dB)
		{
			var result = await dB.Pokemons.ToListAsync();

			if (result == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		/// <summary>
		///		Updates each row with names.
		/// </summary>
		/// <param name="dB"></param>
		/// <returns></returns>
		static public async Task<IResult> UpdateDB(VotemonDbContext dB)
		{
			for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
			{
				var pokemon = await dB.Pokemons.ElementAtAsync(i + 1);
				pokemon.Name = PokeApiMethods.PokemonNames[i];
			}
			await dB.SaveChangesAsync();

			return VotemonDbGateway.ReadDbPokemon(dB).Result;
		}

		/// <summary>
		///		Inserts database 
		/// </summary>
		/// <param name="dB"></param>
		/// <returns></returns>
		static public async Task<IResult> InsertIntoDB(VotemonDbContext dB)
		{
			foreach (var name in PokeApiMethods.PokemonNames)
			{
				var pokemon = new Models.Pokemon();
				pokemon.Name = name;
				await dB.AddAsync(pokemon);
			}
			await dB.SaveChangesAsync();

			return VotemonDbGateway.ReadDbPokemon(dB).Result;
		}


		
	}
}
