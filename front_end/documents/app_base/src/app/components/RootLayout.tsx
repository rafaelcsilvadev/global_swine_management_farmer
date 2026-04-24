import { Outlet } from "react-router";
import { OfflineIndicator } from "./OfflineIndicator";
import { BottomNav } from "./BottomNav";
import { ContextSelectorBar } from "./ContextSelectorBar";
import { ContextSelectorDrawer } from "./ContextSelectorDrawer";
import { FarmProvider } from "../context/FarmContext";

export function RootLayout() {
  return (
    <FarmProvider>
      <div className="flex flex-col h-screen bg-gray-50 max-w-md mx-auto relative overflow-hidden">
        {/* Offline indicator always visible at top */}
        <OfflineIndicator />

        {/* Context selector bar — always visible */}
        <ContextSelectorBar />

        {/* Page content */}
        <div className="flex-1 flex flex-col overflow-hidden">
          <Outlet />
        </div>

        {/* Bottom navigation */}
        <BottomNav />

        {/* Context selector drawer (portal-like overlay) */}
        <ContextSelectorDrawer />
      </div>
    </FarmProvider>
  );
}
