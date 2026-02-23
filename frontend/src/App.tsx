/* import { useState } from "react"; */
import Header from "./components/Navigation";
import "./App.css";
import Battle from "./components/Battle";

function App() {
  return (
    <>
      <header>
        <Header />
      </header>
      <main>
        <Battle />
      </main>
    </>
  );
}

export default App;
