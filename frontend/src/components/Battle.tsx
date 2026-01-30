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
      console.error("Error recording vote:", err)
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
      <div className={styles.header}>
        <RoundCounter rounds={rounds} />

        <div className={styles.dailyInfo}>
          <span>Daily Challenge • Resets at midnight</span>
          {hasFinishedDaily && (
            <span className={styles.completedBadge}>✓ Completed today</span>
          )}
        </div>

        <button
          onClick={resetDailyProgress}
          className={styles.resetButton}
          title="Reset daily progress (for testing)">
          🔄
        </button>
      </div>

      <h1 className={styles.title}>Vilken är bäst?</h1>

      <div className={styles.pokemonContainer}>
        <PokemonCard
          pokemon={pokemon1}
          onVote={() => addVote(pokemon1.Id)}
          disabled={hasFinishedDaily}
        />

        <img
          className={styles.vsSign}
          src="/src/assets/vs-sign.png"
          alt="Versus sign"
        />

        <PokemonCard
          pokemon={pokemon2}
          onVote={() => addVote(pokemon2.Id)}
          disabled={hasFinishedDaily}
        />
      </div>

      {/* Rendera modalen när showModal är sant */}
      {showModal && <FinishedModal onClose={() => setShowModal(false)} />}
    </div>
  );
}
