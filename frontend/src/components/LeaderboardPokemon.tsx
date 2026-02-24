import type { PokemonGetDTO } from "../types/pokemonGetDTO";

export interface LeaderboardPokemonProps {
  pokemon: PokemonGetDTO;
}

const LeaderboardPokemon: React.FC<LeaderboardPokemonProps> = ({ pokemon }) => {
  return (
    <li>
      <p>{pokemon.dexId}</p>
      <p>{pokemon.name}</p>
    </li>
  );
};

export default LeaderboardPokemon;
