import { createBrowserRouter } from "react-router";
import Leaderboard from "./pages/Leaderboard";
import App from "./App";
import NotFoundPage from "./pages/NotFoudPage";
import Battle from "./components/Battle";

const router = createBrowserRouter ([
    {
        path: "/",
        element: <App />,
        errorElement:<NotFoundPage />,
        children: [
            {
                index: true,
                element: <Battle />,
            },
            {
                path: "leaderboard",
                element: <Leaderboard />,
            }
        ]
    }
])

export default router;