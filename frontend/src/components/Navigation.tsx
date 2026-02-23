import { useState } from "react";
import styles from "./Navigation.module.css";
import ThemeToggle from "./ThemeToggle";
import { useTheme } from "../context/ThemeContext";

export default function Navigation() {
  const { theme } = useTheme();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const logoSrc =
    theme === "light"
      ? "/src/assets/vtmnlogo_light_landscape.svg"
      : "/src/assets/vtmnlogo_dark_landscape.svg";

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
        <p className={styles.navlink} data-tooltip="Coming Soon!">
          Leaderboards
        </p>
        <p className={styles.navlink} data-tooltip="Coming Soon!">
          Battle
        </p>
        <ThemeToggle />
      </div>
    </nav>
  );
}
