import { NavLink } from "react-router";
import { useState, useEffect } from "react";
import LeaderboardPokemon from "../components/LeaderboardPokemon";
import type { Pokemon } from "../types/pokemon";

const Leaderboard = () => {
  const [pokemons, setPokemons] = useState<Pokemon[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect (() => {
    const fetchPokemons = async () => {
      try {
        const response = await fetch(url);
        if (!response.ok) {
          throw new Error ("Kunde inte hämta datan");
        }

        const data = await response.json();
        setPokemons(data.results);
      } catch (error) {
        console.error("Leaderboard listan gick ej att hämta!", error);
      } finally {
        setLoading(false);
      }
    }

    fetchPokemons();
  }, []);


  // lägg en end-point på 100

  return (
    <div>
      <ul className="leaderboard-list">
        {loading ? (
          <p>Loading...</p>
        ) : pokemons.length > 0 ? (
          pokemons.map((pokemon) => (
            <LeaderboardPokemon key={pokemon.Id} pokemon={pokemon} />
          ))
        ) : (
          <p>No pokemons on the leaderboard. Check the console for more information</p>
        )}
      </ul>
      <button>
        <NavLink to="/">Kör igen</NavLink>
      </button>
    </div>
  );
};

export default Leaderboard;
