import type { Pokemon } from "../types/pokemon";

//Fetch Pokemon data som ska visas
export const getPokemon: Function = async (): Promise<Pokemon> => {
  try {
    const randomId = Math.floor(Math.random() * 1025) + 1;

    const response = await fetch(
      `https://pokeapi.co/api/v2/pokemon/${randomId}`
    );

    if (!response.ok) {
      throw new Error("Något gick fel vid hämtning av data!");
    }

    const data = await response.json();

    // Returnera ett enskilt Pokémon-objekt
    return {
      Id: data.id, // Lägg till slumpmässigt nummer för unikt Id
      Name: data.name,
      Url: `https://pokeapi.co/api/v2/pokemon/${data.id}/`,
      ImageUrl:
        data.sprites.other["official-artwork"].front_default ||
        `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/${data.id}.png`,
    };
  } catch (error) {
    console.error("Ett fel inträffade", error);
    throw error;
  }
};
