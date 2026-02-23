using BackendAPI.DTOs;
using System.Text.Json;

namespace BackendAPI.Services
{
	public static class PokeApiMethods
	{
		static public Uri PokeApiPokemonPath { get; private set; } = new Uri("https://pokeapi.co/api/v2/pokemon/");
		static public int NumbOfPokemons { get; private set; } = 1025;
		static public List<string> PokemonNames { get; private set; } = new();


		static public void SetApiPath(string newString)
		{
			if (!string.IsNullOrWhiteSpace(newString))
				PokeApiMethods.PokeApiPokemonPath = new Uri(newString);
		}

		static public void SetNumberOfPokemons(int newNumb)
		{
			if (newNumb > 0)
				PokeApiMethods.NumbOfPokemons = newNumb;
		}

		static public async Task SetPokemonNames()
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = PokeApiMethods.PokeApiPokemonPath;

				for (int i = 1; i <= PokeApiMethods.NumbOfPokemons; i++)
				{
					var response = await client.GetStringAsync($"{i}/");
					var name = JsonSerializer.Deserialize<PokemonNameDTO>(response.ToString())!.Name;

					PokeApiMethods.PokemonNames.Add(name);
					// Console.WriteLine($"{i,4} - {name}");
				}
			}
		}
	}
}
