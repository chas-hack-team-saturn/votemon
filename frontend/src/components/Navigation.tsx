import styles from "./Navigation.module.css";

export default function Navigation() {
  return (
    <nav className={styles.header}>
      <a className={styles.navlink} href="">
        PokéScrandle
      </a>
      <a className={styles.navlink} href="">
        Leaderboards
      </a>
    </nav>
  );
}
