import type { Pokemon } from "../types/pokemon";

export interface LeaderboardPokemonProps {
    pokemon: Pokemon;
}

const LeaderboardPokemon: React.FC<LeaderboardPokemonProps> = ({pokemon}) => {

    return (
        <li>
            <p>{pokemon.Id}</p>
            <p>{pokemon.Name}</p>
        </li>
    )
}

export default LeaderboardPokemon;