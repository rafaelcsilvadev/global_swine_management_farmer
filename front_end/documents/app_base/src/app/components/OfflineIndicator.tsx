import { useState, useEffect } from "react";
import { Wifi, WifiOff, RefreshCw, CheckCircle2 } from "lucide-react";

export function OfflineIndicator() {
  const [isOnline, setIsOnline] = useState(true);
  const [pendingCount, setPendingCount] = useState(0);
  const [syncing, setSyncing] = useState(false);

  useEffect(() => {
    // Simulate random offline state for demo
    const interval = setInterval(() => {
      // Keep as is for demo — user can toggle via button
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  const handleToggle = () => {
    if (isOnline && pendingCount === 0) {
      setPendingCount(3);
      setIsOnline(false);
    } else if (!isOnline) {
      setIsOnline(true);
      setSyncing(true);
      setTimeout(() => {
        setSyncing(false);
        setPendingCount(0);
      }, 2000);
    } else {
      setPendingCount(0);
    }
  };

  if (syncing) {
    return (
      <div className="flex items-center gap-2 px-4 py-2 bg-blue-500 text-white text-sm font-medium">
        <RefreshCw size={14} className="animate-spin" />
        <span>Sincronizando dados... aguarde</span>
      </div>
    );
  }

  if (!isOnline) {
    return (
      <button
        onClick={handleToggle}
        className="w-full flex items-center gap-2 px-4 py-2 bg-amber-500 text-white text-sm font-medium"
      >
        <WifiOff size={14} />
        <span className="flex-1 text-left">
          Sem conexão — {pendingCount} apontamento{pendingCount !== 1 ? "s" : ""} pendente{pendingCount !== 1 ? "s" : ""}
        </span>
        <span className="text-xs bg-white/20 rounded px-2 py-0.5">Toque para sincronizar</span>
      </button>
    );
  }

  return (
    <button
      onClick={handleToggle}
      className="w-full flex items-center gap-2 px-4 py-2 bg-green-600 text-white text-sm font-medium"
    >
      <CheckCircle2 size={14} />
      <span className="flex-1 text-left">Conectado — Dados sincronizados</span>
      <Wifi size={14} />
    </button>
  );
}
