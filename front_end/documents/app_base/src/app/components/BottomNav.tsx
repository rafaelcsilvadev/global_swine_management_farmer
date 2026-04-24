import { useNavigate, useLocation } from "react-router";
import { Home, Baby, Heart, Wheat, ClipboardCheck } from "lucide-react";

const navItems = [
  { path: "/", label: "Início", icon: Home },
  { path: "/maternidade", label: "Partos", icon: Baby },
  { path: "/consumo", label: "Consumo", icon: Wheat },
  { path: "/saude", label: "Saúde", icon: Heart },
  { path: "/checkup", label: "Checkup", icon: ClipboardCheck },
];

export function BottomNav() {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <nav className="fixed bottom-0 left-0 right-0 bg-white border-t-2 border-gray-100 flex z-50 max-w-md mx-auto shadow-lg">
      {navItems.map(({ path, label, icon: Icon }) => {
        const isActive = location.pathname === path;
        return (
          <button
            key={path}
            onClick={() => navigate(path)}
            className={`flex-1 flex flex-col items-center justify-center py-2 gap-0.5 transition-all ${
              isActive ? "text-green-600" : "text-gray-400"
            }`}
          >
            <div
              className={`p-1.5 rounded-xl transition-all ${
                isActive ? "bg-green-50" : ""
              }`}
            >
              <Icon size={22} strokeWidth={isActive ? 2.5 : 1.8} />
            </div>
            <span className="text-xs font-medium">{label}</span>
          </button>
        );
      })}
    </nav>
  );
}
