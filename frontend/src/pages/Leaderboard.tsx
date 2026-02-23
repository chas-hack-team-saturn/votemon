import { NavLink } from "react-router";

const Leaderboard = () => {


    return (
        <div>
            <ul className="leaderboard-list">
                <li className="User-1">Emil56</li>
            </ul>
            <button><NavLink to="/">Kör igen</NavLink></button>
        </div>
    )
}

export default Leaderboard;