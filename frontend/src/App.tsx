/* import { useState } from "react"; */
import Header from "./components/Navigation";
import "./App.css";
import { Outlet } from "react-router";

function App() {
  return (
    <>
      <Header />
      <main>
        {/*Outlet fungerar som en sittplats där man själv får välja vad som ska visas genom router filen*/}
        <Outlet />
      </main>
    </>
  );
}

export default App;
