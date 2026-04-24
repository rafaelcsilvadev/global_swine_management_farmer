import { createBrowserRouter } from "react-router";
import { RootLayout } from "./components/RootLayout";
import HomeScreen from "./screens/HomeScreen";
import MaternidadeScreen from "./screens/MaternidadeScreen";
import SaudeScreen from "./screens/SaudeScreen";
import ConsumoScreen from "./screens/ConsumoScreen";
import CheckupScreen from "./screens/CheckupScreen";

export const router = createBrowserRouter([
  {
    path: "/",
    Component: RootLayout,
    children: [
      { index: true, Component: HomeScreen },
      { path: "maternidade", Component: MaternidadeScreen },
      { path: "saude", Component: SaudeScreen },
      { path: "consumo", Component: ConsumoScreen },
      { path: "checkup", Component: CheckupScreen },
    ],
  },
]);
