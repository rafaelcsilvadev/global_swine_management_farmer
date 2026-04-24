import { Minus, Plus } from "lucide-react";

interface StepperProps {
  value: number;
  onChange: (value: number) => void;
  min?: number;
  max?: number;
  label: string;
  sublabel?: string;
  icon?: React.ReactNode;
  variant?: "default" | "alert" | "water" | "feed";
}

const variantStyles = {
  default: {
    container: "bg-white border-2 border-gray-200",
    minus: "bg-gray-100 text-gray-700 active:bg-gray-200",
    plus: "bg-green-600 text-white active:bg-green-700",
    value: "text-gray-900",
  },
  alert: {
    container: "bg-red-50 border-2 border-red-200",
    minus: "bg-red-100 text-red-700 active:bg-red-200",
    plus: "bg-red-600 text-white active:bg-red-700",
    value: "text-red-700",
  },
  water: {
    container: "bg-blue-50 border-2 border-blue-200",
    minus: "bg-blue-100 text-blue-700 active:bg-blue-200",
    plus: "bg-blue-600 text-white active:bg-blue-700",
    value: "text-blue-700",
  },
  feed: {
    container: "bg-amber-50 border-2 border-amber-200",
    minus: "bg-amber-100 text-amber-700 active:bg-amber-200",
    plus: "bg-amber-500 text-white active:bg-amber-600",
    value: "text-amber-700",
  },
};

export function Stepper({
  value,
  onChange,
  min = 0,
  max = 999,
  label,
  sublabel,
  icon,
  variant = "default",
}: StepperProps) {
  const styles = variantStyles[variant];

  const decrement = () => {
    if (value > min) onChange(value - 1);
  };

  const increment = () => {
    if (value < max) onChange(value + 1);
  };

  return (
    <div className={`rounded-2xl p-4 ${styles.container}`}>
      <div className="flex items-center gap-2 mb-3">
        {icon && <span className="text-2xl">{icon}</span>}
        <div>
          <p className="font-semibold text-gray-900">{label}</p>
          {sublabel && <p className="text-xs text-gray-500">{sublabel}</p>}
        </div>
      </div>
      <div className="flex items-center gap-3">
        <button
          onPointerDown={decrement}
          disabled={value <= min}
          className={`flex-1 h-16 rounded-xl flex items-center justify-center transition-all active:scale-95 disabled:opacity-30 ${styles.minus}`}
        >
          <Minus size={28} strokeWidth={3} />
        </button>
        <div className="flex-1 flex flex-col items-center justify-center">
          <span className={`text-5xl font-black tabular-nums ${styles.value}`}>
            {value}
          </span>
        </div>
        <button
          onPointerDown={increment}
          disabled={value >= max}
          className={`flex-1 h-16 rounded-xl flex items-center justify-center transition-all active:scale-95 disabled:opacity-30 ${styles.plus}`}
        >
          <Plus size={28} strokeWidth={3} />
        </button>
      </div>
    </div>
  );
}
