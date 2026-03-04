namespace BackendAPI.Services
{
	/// <summary>
	///     Utility class for generating MySQL database scripts.
	/// </summary>
	/// <remarks>
	///		Use these scripts when <see cref="Endpoints"/> methods are not working.
	/// </remarks>
	static public class GenerateScript
	{
		/// <summary>
		///		Generates an insert.sql script that inserts all pokemons into Pokemon table.
		/// </summary>
		/// <remarks>
		///		<para>Will overwrite existing insert.sql script.</para>
		///		Requires <see cref="PokeApiMethods.PokemonNames"/> to be filled before invoking this method.
		/// </remarks>
		static internal void InsertIntoPokemon()
		{
			using (var writer = new StreamWriter("dataseed.sql", false))
			{
				writer.WriteLine("USE Votemon");
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

        /// <summary>
        ///		Generates update.sql script that updates each row's Name column in Pokemon table.
        /// </summary>
        /// <remarks>
		///		<para>Will overwrite existing update.sql script.</para>
        ///		Requires <see cref="PokeApiMethods.PokemonNames"/> to be filled before invoking this method.
        /// </remarks>
        static internal void UpdateAllPokemonNames()
		{
			using (var writer = new StreamWriter("update.sql", false))
			{
				writer.WriteLine("USE Votemon");

				for (int i = 0; i < PokeApiMethods.PokemonNames.Count; i++)
				{
					writer.WriteLine(
						$"UPDATE Pokemon SET Name = '{PokeApiMethods.PokemonNames[i]}' WHERE DexId = {i + 1};"
						);
				}
			}
		}
	}
}
