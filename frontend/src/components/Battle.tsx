import { useState, useEffect } from "react";
import RoundCounter from "./RoundCounter";
import styles from "./Battle.module.css";
import { PokemonCard } from "./PokemonCard";
import { getPokemon } from "../api/api";
import type { Pokemon } from "../types/pokemon";
import FinishedModal from "../components/FinishedModal";
import { storage } from "../utils/storage"; // Import the storage utility

export interface Rounds {
  totalRounds: number;
  currentRound: number;
}

export default function Battle() {
  const [hasFinishedDaily, setHasFinishedDaily] = useState<boolean>(false);
  const [rounds, setRounds] = useState<Rounds>({
    totalRounds: 10,
    currentRound: 1,
  });

  const [pokemon1, setPokemon1] = useState<Pokemon | null>(null);
  const [pokemon2, setPokemon2] = useState<Pokemon | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState<boolean>(false); // Hantera modalen utan isModalVisible

  // Initialize storage and check daily progress
  useEffect(() => {
    storage.initialize();
    const finished = storage.getHasFinishedDaily();
    setHasFinishedDaily(finished);

    // If already finished today, set to final round and show modal
    if (finished) {
      setRounds((prev) => ({ ...prev, currentRound: 10 }));
      // Show the modal immediately for returning users
      setShowModal(true); // Visa modalen direkt
    }
  }, []);

  // Show finishedModal when hasFinishedDaily becomes true (for new completions)
  useEffect(() => {
    if (hasFinishedDaily) {
      setShowModal(true); // Visa modalen direkt
    }
  }, [hasFinishedDaily]);

  const fetchRandomPokemon = async () => {
    try {
      setIsLoading(true);
      const [firstPokemon, secondPokemon] = await Promise.all([
        getPokemon(),
        getPokemon(),
      ]);
      setPokemon1(firstPokemon);
      setPokemon2(secondPokemon);
      setError(null);
    } catch (err) {
      setError("Failed to fetch Pokémon");
      console.error("Error fetching Pokémon:", err);
    } finally {
      setIsLoading(false);
    }
  };

  const advanceRound = () => {
    if (rounds.currentRound < rounds.totalRounds) {
      setRounds((prev) => ({
        ...prev,
        currentRound: prev.currentRound + 1,
      }));
      fetchRandomPokemon();
    } else {
      // Game completed - mark as finished for today
      storage.setHasFinishedDaily(true);
      setHasFinishedDaily(true);
      // Note: finishedModal() will be called by the useEffect above

      setShowModal(true); // Visa modalen direkt
      console.log("Game completed!");
    }
  };

  // Add vote for a specific Pokémon
  const addVote = (pokemonId: number) => {
    // Show modal if already finished for today but still allow UI interaction
    if (hasFinishedDaily) {
      setShowModal(true); // Visa modalen om det redan har avslutats
      return;
    }

    fetch(`backend/vote=${pokemonId}`, { method: "Put" }).catch((err) =>
      console.error("Error recording vote:", err),
    );

    console.log(`Voted for Pokémon with ID: ${pokemonId}`);
    advanceRound();
  };

  // Add a reset function if needed (for testing)
  const resetDailyProgress = () => {
    storage.resetDailyProgress();
    setHasFinishedDaily(false);
    setRounds((prev) => ({ ...prev, currentRound: 1 }));
    fetchRandomPokemon();
    setShowModal(false);
  };

  // Initial fetch on component mount
  useEffect(() => {
    fetchRandomPokemon();
  }, []);

  // Add an effect to check for day changes periodically
  useEffect(() => {
    const checkDayChange = () => {
      if (storage.isNewDay()) {
        storage.resetDailyProgress();
        setHasFinishedDaily(false);
        setRounds((prev) => ({ ...prev, currentRound: 1 }));

        // Refresh Pokémon if they exist
        if (pokemon1 && pokemon2) {
          fetchRandomPokemon();
        }
      }
    };

    // Check every minute for day changes
    const interval = setInterval(checkDayChange, 60000);

    return () => clearInterval(interval);
  }, [pokemon1, pokemon2]);

  if (isLoading) {
    return <div className={styles.loading}>Loading Pokémon...</div>;
  }

  if (error) {
    return (
      <div className={styles.error}>
        <p>{error}</p>
        <button onClick={fetchRandomPokemon}>Try Again</button>
      </div>
    );
  }

  // Don't render until we have Pokémon data
  if (!pokemon1 || !pokemon2) {
    return null;
  }

  return (
    <div className={styles.battle}>
      {/* Header with progress and reset button */}

      <div className={styles.pokemonContainer}>
        <PokemonCard
          pokemon={pokemon1}
          onVote={() => addVote(pokemon1.Id)}
          disabled={hasFinishedDaily}
        />

        <svg className={styles.vsSign} viewBox="0 0 156.42 124.9">
          <path d="M22.5,97.18L67.4,5.56c1.22-2.43,3.32-3.54,6.31-3.32l12.95.5c1.66,0,2.88.5,3.65,1.49,1.11,1,1.11,2.55,0,4.65l-55.44,106.73c-.5.89-.97,1.66-1.41,2.32-2.16,2.88-4.73,4.32-7.72,4.32l-19.04.17c-1.82-.11-3.25-.64-4.3-1.58-1.6-1.38-2.32-3.51-2.15-6.39L6.39,6.06c.11-2.38,1.76-3.6,4.96-3.65h13.39c1.1,0,2.01.3,2.73.91.83.66,1.18,1.85,1.07,3.56l-6.03,90.3Z" />
          <path d="M133.44,0c2.31,0,4.48.28,6.51.84,11.15,3.23,16.57,14.47,16.23,33.72-.06,2.89-1.37,4.67-3.95,5.34l-12.02,3.17c-.6.17-1.29.22-2.06.17-2.36-.28-3.6-1.49-3.71-3.65-.39-9.18-2.14-15.8-5.26-19.84-.39,0-.76.08-1.09.25-1.34.78-2.91,2.66-4.7,5.64-3.7,6.03-5.54,11.67-5.54,16.93,0,2.6.5,4.59,1.51,5.98,1.12,1.44,3.7,2.71,7.74,3.82l3,.83c3.06.83,5.51,1.88,7.35,3.15,4.8,3.38,7.2,9.85,7.2,19.42,0,4.81-.86,9.74-2.57,14.77-3.16,9.35-8.36,17.35-15.61,23.98-7.58,6.92-15.31,10.37-23.17,10.37-4.06,0-7.69-.69-10.87-2.07-9.22-4.04-13.78-13.47-13.67-28.3,0-.61.05-1.24.16-1.91.49-3.26,1.76-5.23,3.79-5.89l12.69-4.48c1.15-.39,2.31-.55,3.46-.5,1.98.17,2.97,1.18,2.97,3.03-.33,13.28.49,21.12,2.45,23.53,1.04,0,1.97-.17,2.8-.5,1.86-.95,3.7-2.61,5.51-4.96,4.22-5.49,6.33-12.02,6.33-19.58,0-3.1-.22-5.37-.66-6.82-.99-2.94-3.8-4.99-8.43-6.15l-2.62-.66c-1.42-.39-2.89-.91-4.42-1.58-7.12-3.37-10.68-8.93-10.68-16.68,0-4.28.72-8.81,2.15-13.59,2.92-9.73,7.77-18.18,14.55-25.35,7.77-8.28,15.98-12.43,24.63-12.43Z" />
        </svg>

        <PokemonCard
          pokemon={pokemon2}
          onVote={() => addVote(pokemon2.Id)}
          disabled={hasFinishedDaily}
        />
      </div>

      <div className={styles.header}>
        <RoundCounter rounds={rounds} />

        <button
          onClick={resetDailyProgress}
          className={styles.resetButton}
          title="Reset daily progress (for testing)"
        >
          🔄
        </button>
      </div>

      {/* Rendera modalen när showModal är sant */}
      {showModal && <FinishedModal onClose={() => setShowModal(false)} />}
    </div>
  );
}
