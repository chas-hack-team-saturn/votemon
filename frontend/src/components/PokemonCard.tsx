import React from "react";
import type { Pokemon } from "../types/pokemon";
import styles from "./PokemonCard.module.css";
import sticker from "../assets/votemon_sticker.svg";

export interface PokemonCardProps {
  pokemon: Pokemon;
  onVote: (id: number) => void; // Add this prop for handling votes
  disabled?: boolean; // Add this line
  showSticker?: boolean; // Add this line
}

export const PokemonCard: React.FC<PokemonCardProps> = ({
  pokemon,
  onVote,
  disabled,
  showSticker,
}) => {
  return (
    <button
      className={styles.pokemonCard}
      onClick={() => onVote(pokemon.Id)} // Use the onVote prop
      disabled={disabled}
    >
      <h2>{pokemon.Name}</h2>
      <img
        className={styles.pokemonImage}
        src={pokemon.ImageUrl}
        alt={pokemon.Name}
      />
      <div className={styles.stats}>
        <p>ELO: {pokemon.EloRating}</p>
      </div>
      {showSticker && (
        <img src={sticker} alt="Voted sticker" className={styles.sticker} />
      )}
    </button>
  );
};

export default PokemonCard;
