import { NavLink } from "react-router";
import { useState, useEffect } from "react";

const Leaderboard = () => {
  // lägg en end-point på 100

  return (
    <div>
      <ul className="leaderboard-list">
        <li className="User-1">Pikachu</li>
      </ul>
      <button>
        <NavLink to="/">Kör igen</NavLink>
      </button>
    </div>
  );
};

export default Leaderboard;
