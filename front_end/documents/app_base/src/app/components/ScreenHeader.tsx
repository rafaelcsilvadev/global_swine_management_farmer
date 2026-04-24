import { useNavigate } from "react-router";
import { ArrowLeft } from "lucide-react";

interface ScreenHeaderProps {
  title: string;
  subtitle?: string;
  showBack?: boolean;
  badge?: string;
  badgeColor?: "green" | "amber" | "red" | "blue";
}

const badgeColors = {
  green: "bg-green-100 text-green-700",
  amber: "bg-amber-100 text-amber-700",
  red: "bg-red-100 text-red-700",
  blue: "bg-blue-100 text-blue-700",
};

export function ScreenHeader({
  title,
  subtitle,
  showBack = true,
  badge,
  badgeColor = "green",
}: ScreenHeaderProps) {
  const navigate = useNavigate();

  return (
    <div className="bg-white border-b-2 border-gray-100 px-4 py-3">
      <div className="flex items-center gap-3">
        {showBack && (
          <button
            onClick={() => navigate(-1)}
            className="p-2 rounded-xl bg-gray-100 active:bg-gray-200 transition-all"
          >
            <ArrowLeft size={22} className="text-gray-700" />
          </button>
        )}
        <div className="flex-1">
          <div className="flex items-center gap-2">
            <h1 className="font-black text-gray-900">{title}</h1>
            {badge && (
              <span className={`text-xs font-bold px-2 py-0.5 rounded-full ${badgeColors[badgeColor]}`}>
                {badge}
              </span>
            )}
          </div>
          {subtitle && (
            <p className="text-xs text-gray-500 mt-0.5">{subtitle}</p>
          )}
        </div>
      </div>
    </div>
  );
}
