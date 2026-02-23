import { useState } from "react";
import styles from "./Navigation.module.css";
import ThemeToggle from "./ThemeToggle";
import { useTheme } from "../context/ThemeContext";

import { NavLink } from "react-router";

import logoLight from "../assets/vtmnlogo_light_landscape.svg";
import logoDark from "../assets/vtmnlogo_dark_landscape.svg";

export default function Navigation() {
  const { theme } = useTheme();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const logoSrc = theme === "light" ? logoLight : logoDark;

  return (
    <nav className={styles.header}>
      <a
        className={styles.navlink}
        href=""
        aria-label="Votémon: the daily voting game"
      >
        <img
          src={logoSrc}
          alt="Votémon: the daily voting game"
          className={styles.logo}
        />
      </a>

      <button
        className={`${styles.hamburger} ${isMenuOpen ? styles.active : ""}`}
        onClick={() => setIsMenuOpen(!isMenuOpen)}
        aria-label="Toggle menu"
        aria-expanded={isMenuOpen}
      >
        <span className={styles.bar}></span>
        <span className={styles.bar}></span>
        <span className={styles.bar}></span>
      </button>

      <div
        className={`${styles.navright} ${isMenuOpen ? styles.menuOpen : ""}`}
      >
        <NavLink className={styles.navlink} to="leaderboard">
          Leaderboards
        </NavLink>
        <NavLink className={styles.navlink} to="">
          Battle
        </NavLink>
        <ThemeToggle />
      </div>
    </nav>
  );
}
