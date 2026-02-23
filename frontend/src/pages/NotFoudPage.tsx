import { NavLink } from "react-router";

const NotFoundPage = () => {
    return (
        <div>
      <h1>404 🧩</h1>
      
      <p>Hoppsan! Den här sidan verkar ha gått vilse i koden.</p>
      
      {/* 🟢 KNAPPEN SOM RÄDDAR ANVÄNDAREN */}
      <NavLink to="/">
        Ta mig tillbaka till tryggheten (Hem)
      </NavLink>
    </div>
    )
}

export default NotFoundPage;