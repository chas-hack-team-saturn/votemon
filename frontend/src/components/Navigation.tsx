import styles from "./Navigation.module.css";
import ThemeToggle from "./ThemeToggle";

export default function Navigation() {
  return (
    <nav className={styles.header}>
      <a className={styles.navlink} href="">
        <img src="/src/assets/vtmnlogo_light_landscape.svg" alt="Votémon: the daily voting game" className={styles.logo} />
      </a>
      <a className={styles.navlink} href="">
        Leaderboards
      </a>
      <ThemeToggle />
    </nav>
  );
}
