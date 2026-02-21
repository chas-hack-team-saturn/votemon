import React from "react";
import type { Pokemon } from "../types/pokemon";
import styles from "./PokemonCard.module.css";

export interface PokemonCardProps {
  pokemon: Pokemon;
  onVote: (id: number) => void; // Add this prop for handling votes
  disabled?: boolean; // Add this line
}

export const PokemonCard: React.FC<PokemonCardProps> = ({
  pokemon,
  onVote,
}) => {
  return (
    <button
      className={styles.pokemonCard}
      onClick={() => onVote(pokemon.Id)} // Use the onVote prop
    >
      <h2>{pokemon.Name}</h2>
      <img
        className={styles.pokemonImage}
        src={pokemon.ImageUrl}
        alt={pokemon.Name}
      />
    </button>
  );
};

export default PokemonCard;
