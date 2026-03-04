using Microsoft.EntityFrameworkCore;
using BackendAPI.Data;

namespace BackendAPI.Services
{
	public static class VotemonDbGateway
	{
		/// <summary>
		///		Returns all records in the <see cref="VotemonDbContext.Pokemons"/> table in database. 
		/// </summary>
		/// <param name="dB">The database to read from.</param>
		/// <returns>
		///		<list type="bullet">
		///			<item> 200 OK status alongside all records in Pokemon table. </item>
		///			<item> 404 NOT FOUND status if nothing is found. </item>
		///		</list>
		/// </returns>
		static public async Task<IResult> ReadDBPokemon(VotemonDbContext dB)
		{
			var result = await dB.Pokemons.OrderBy(p => p.DexId).ToListAsync();

			if (result == null || result.Count <= 0) {
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		/// <summary>
		///		Returns a single record from <see cref="VotemonDbContext.Pokemons"/>.
		/// </summary>
		/// <param name="dB">The database to read from.</param>
		/// <param name="dexId">Used for searching the record with a matching id.</param>
		/// <returns>
		///		<list type="bullet">
		///			<item> 200 OK status, alongside the matching record. </item>
		///			<item> 404 NOT FOUND status if nothing is found. </item>
		///		</list>
		/// </returns>
		static public async Task<IResult> ReadDBPokemon(VotemonDbContext dB, int dexId)
		{
			var result = await dB.Pokemons.FirstOrDefaultAsync(p => p.DexId == dexId);
			
			if (result == null) {
				return Results.NotFound();
			}

			return Results.Ok(result);
		}


		/// <summary>
		///		Returns the top <paramref name="returnedRows"/> with the highest ELO rating from <see cref="VotemonDbContext.Pokemons"/>.
		/// </summary>
		/// <remarks>
		///		Use <paramref name="skippedRows"/> to skip records. Perfect for having multiple leaderboard pages.
		///		<list type="bullet">
		///			<listheader>Here's an example: </listheader>
		///			<item>
		///				<term> Page 1 </term>
		///				<description> <paramref name="skippedRows"/> is set to 0 </description>
		///			</item>
		///			<item>
		///				<term> Page 2 </term>
		///				<description> <paramref name="skippedRows"/> is set to 100 </description>
		///			</item>
		///			<item>
		///				<term> Page 3 </term>
		///				<description> <paramref name="skippedRows"/> is set to 200 </description>
		///			</item>
		///			<item> Etc... </item>
		///		</list>
		///		<para> The formula would be: Page <i>n</i> × <paramref name="returnedRows"/> = <paramref name="skippedRows"/> </para>
		/// </remarks>
		/// <param name="dB"> The database to read from. </param>
		/// <param name="skippedRows"> Optional. The amount of records to be skipped. Default none. </param>
		/// <param name="returnedRows"> Optional. The amount of records to be return. Default 100. </param>
		/// <returns>
		///		<list type="bullet">
		///			<item> 200 OK status, alongside <paramref name="returnedRows"/> amount of records. </item>
		///			<item> 404 NOT FOUND status if nothing is found. </item>
		///		</list>
		/// </returns>
		static public async Task<IResult> ReadDBPokemonELORating(VotemonDbContext dB, int skippedRows = 0, int returnedRows = 100)
		{
			if (skippedRows <= 0) {
				skippedRows = 0;
			}
			if (returnedRows <= 0 || returnedRows > 1000) { 
				returnedRows = 100; 
			}
			
			var result = await dB.Pokemons
				.OrderByDescending(p => p.EloRating)
				.Skip(skippedRows)
				.Take(returnedRows)
				.ToListAsync();

			if (result.Count <= 0 || result == null) {
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		/// <summary>
		///		Adds a vote to Pokemon with <paramref name="winnerDexId"/>, as well as updating both Pokemon ELO rating. 
		/// </summary>
		/// <remarks>
		///		The picked Pokemon gains higher ELO rating while the not picked Pokemon loses ELO rating. 
		///		The gain and loss is scaled based on the ELO rating difference between the two Pokemons.
		///		<list type="bullet">
		///			<item> The Pokemon with a higher ELO rating gains less and loses more. </item>
		///			<item> The Pokemon with a lesser ELO rating gains more and loses less. </item>
		///		</list>
		/// </remarks>
		/// <param name="dB"> The database to update. </param>
		/// <param name="winnerDexId"> The Pokemon DexId that the user picked. </param>
		/// <param name="loserDexId"> The Pokemon DexId that the user DID NOT pick. </param>
		/// <returns>
		///		<list type="bullet">
		///			<item> 200 OK status on successful update. </item>
		///			<item> 404 NOT FOUND status if <paramref name="loserDexId"/> or <paramref name="winnerDexId"/> are invalid. </item>
		///		</list>
		/// </returns>
		static public async Task<IResult> UpdateDBPokemonBattle(VotemonDbContext dB, int winnerDexId, int loserDexId) 
		{
			var winner = await dB.Pokemons.FirstOrDefaultAsync(p => p.DexId == winnerDexId);
			var loser = await dB.Pokemons.FirstOrDefaultAsync(p => p.DexId == loserDexId);

			if (winner == null || loser == null) {
				return Results.NotFound();
			}

			double expectedWinner = 1.0 / (1.0 + Math.Pow(10, (double)(loser.EloRating! - winner.EloRating!) / 400.0));
			double expectedLoser = 1.0 / (1.0 + Math.Pow(10, (double)(winner.EloRating! - loser.EloRating!) / 400.0));
			
			int kFactor = 32;
			winner.EloRating = (int)Math.Round((double)winner.EloRating! + kFactor * (1 - expectedWinner));
			loser.EloRating = (int)Math.Round((double)loser.EloRating! + kFactor * (0 - expectedLoser));

			winner.Votes++;

			await dB.SaveChangesAsync();
			return Results.Ok();
		}


		/// <summary>
		///		Adds a vote to the Pokemon with <paramref name="dexId"/>.
		/// </summary>
		/// <param name="dB"> The database to update. </param>
		/// <param name="dexId"> The DexId of the Pokemon. </param>
		/// <returns>
		///		<list type="bullet">
		///			<item> 200 OK status. </item>
		///			<item> 404 NOT FOUND status if <paramref name="dexId"/> is invalid. </item>
		///		</list>
		/// </returns>
		static public async Task<IResult> UpdateDBPokemonVote(VotemonDbContext dB, int dexId)
		{
			var result = await dB.Pokemons.FirstOrDefaultAsync(p => p.DexId == dexId);
			if (result == null) {
				return Results.NotFound();
			}
			result.Votes++;
			await dB.SaveChangesAsync();
			return Results.Ok(result);
		}



		/// <summary>
		///		Updates every record names in <see cref="VotemonDbContext.Pokemons"/> table with entries from <see cref="PokeApiMethods.PokemonNames"/>.
		/// </summary>
		/// <param name="dB">The database to update.</param>
		/// <returns>
		///		<list type="bullet">
		///			<item>200 OK status on successful update, alongside all records in Pokemon table.</item>
		///			<item>404 NOT FOUND status if <see cref="PokeApiMethods.PokemonNames"/> is empty.</item>
		///		</list>
		/// </returns>
		static public async Task<IResult> UpdateDBPokemonNames(VotemonDbContext dB)
		{
			if (PokeApiMethods.PokemonNames.Count <= 0 || PokeApiMethods.PokemonNames == null) {
				return Results.InternalServerError($"{PokeApiMethods.PokemonNames} has no entries");
			}

			for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
			{
				var pokemon = await dB.Pokemons.ElementAtAsync(i);
				pokemon.Name = PokeApiMethods.PokemonNames[i];
			}
			await dB.SaveChangesAsync();

			return VotemonDbGateway.ReadDBPokemon(dB).Result;
		}

		/// <summary>
		///		Creates new data records in <see cref="VotemonDbContext.Pokemons"/> table using entries from <see cref="PokeApiMethods.PokemonNames"/>.
		/// </summary>
		/// <param name="dB">The database to insert new entries to.</param>
		/// <returns>
		///		<list type="bullet">
		///			<item>200 OK status on successful insert, alongside all records in Pokemon table.</item>
		///			<item>404 NOT FOUND status if <see cref="PokeApiMethods.PokemonNames"/> is empty.</item>
		///		</list>
		///	</returns>
		static public async Task<IResult> InsertDBPokemonNames(VotemonDbContext dB)
		{
			if (PokeApiMethods.PokemonNames.Count <= 0 || PokeApiMethods.PokemonNames == null)
			{
				return Results.InternalServerError($"{PokeApiMethods.PokemonNames} has no entries");
			}

			foreach (var name in PokeApiMethods.PokemonNames)
			{
				var pokemon = new Models.Pokemon();
				pokemon.Name = name;
				await dB.AddAsync(pokemon);
			}
			await dB.SaveChangesAsync();

			return Results.Created("https://votemon.pabu.dev/get/", dB.Pokemons);
		}


		
	}
}
