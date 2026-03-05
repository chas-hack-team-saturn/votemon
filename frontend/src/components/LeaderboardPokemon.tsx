import type { PokemonGetDTO } from "../types/pokemonGetDTO";
import styles from "./LeaderboardPokemon.module.css";

export interface LeaderboardPokemonProps {
  pokemon: PokemonGetDTO;
}

const LeaderboardPokemon: React.FC<LeaderboardPokemonProps> = ({ pokemon }) => {
  return (
    <li className={styles.leaderboardItem}>
      <p>Id: {pokemon.dexId}</p>
      <p>Name: {pokemon.name}</p>
      <p>Votes: {pokemon.votes}</p>
      <p>Elo: {pokemon.eloRating}</p>
    </li>
  );
};

export default LeaderboardPokemon;
