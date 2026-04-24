import { useState } from "react";
import { useNavigate } from "react-router";
import { CheckCircle2, TrendingUp, TrendingDown, Minus } from "lucide-react";
import { ScreenHeader } from "../components/ScreenHeader";
import { Stepper } from "../components/Stepper";
import { useFarm } from "../context/FarmContext";

export default function ConsumoScreen() {
  const navigate = useNavigate();
  const { selection } = useFarm();
  const { galpao, lote, animal } = selection;

  const [racao, setRacao] = useState(0);
  const [agua, setAgua] = useState(0);
  const [saved, setSaved] = useState(false);

  // Reference values for comparison
  const racaoRef = 3;
  const aguaRef = 8;

  const getRacaoStatus = () => {
    if (racao === 0) return null;
    if (racao < racaoRef - 0.5) return "baixo";
    if (racao > racaoRef + 0.5) return "alto";
    return "normal";
  };

  const getAguaStatus = () => {
    if (agua === 0) return null;
    if (agua < aguaRef - 1) return "baixo";
    if (agua > aguaRef + 1) return "alto";
    return "normal";
  };

  const statusInfo: Record<string, { label: string; color: string; icon: React.ElementType }> = {
    baixo: { label: "Abaixo do normal", color: "text-red-600", icon: TrendingDown },
    alto: { label: "Acima do normal", color: "text-amber-600", icon: TrendingUp },
    normal: { label: "Consumo normal", color: "text-green-600", icon: Minus },
  };

  const racaoStatus = getRacaoStatus();
  const aguaStatus = getAguaStatus();

  const handleConfirm = () => {
    setSaved(true);
    setTimeout(() => navigate("/"), 1800);
  };

  if (saved) {
    return (
      <div className="flex flex-col items-center justify-center h-full bg-green-50 px-6 text-center gap-4">
        <div className="w-24 h-24 bg-green-100 rounded-full flex items-center justify-center">
          <CheckCircle2 size={48} className="text-green-600" />
        </div>
        <h2 className="font-black text-gray-900">Consumo Registrado!</h2>
        <p className="text-gray-500 text-sm">
          Ração: {racao} sacos · Água: {agua} caixas d'água
        </p>
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full">
      <ScreenHeader
        title="Consumo de Insumos"
        subtitle="Registrar ração e água do lote"
      />

      <div className="flex-1 overflow-y-auto px-4 py-4 pb-36 bg-gray-50 flex flex-col gap-4">
        {/* Lote Card */}
        <div className="bg-white border-2 border-gray-100 rounded-2xl p-4 flex items-center gap-3">
          <span className="text-3xl">{galpao.emoji}</span>
          <div>
            <p className="font-black text-gray-900">{lote.label}</p>
            <p className="text-xs text-gray-500">
              {lote.animals} animais · {galpao.type} · {galpao.label} · Dia {lote.daysActive}
            </p>
            <p className="text-xs text-gray-400 mt-0.5">{animal.label} · {animal.detail}</p>
          </div>
        </div>

        {/* Racao Section */}
        <div>
          <p className="font-black text-gray-900 mb-3 flex items-center gap-2">
            <span className="text-xl">🌾</span> Ração
          </p>
          <Stepper
            value={racao}
            onChange={setRacao}
            label="Sacos de Ração"
            sublabel="Referência: ~3 sacos/dia para este lote"
            icon="🌾"
            variant="feed"
          />
          {racaoStatus && (
            <div className={`mt-2 flex items-center gap-1.5 px-3 py-1.5 rounded-xl ${
              racaoStatus === "normal" ? "bg-green-50" : racaoStatus === "baixo" ? "bg-red-50" : "bg-amber-50"
            }`}>
              {(() => {
                const info = statusInfo[racaoStatus];
                const Icon = info.icon;
                return (
                  <>
                    <Icon size={14} className={info.color} />
                    <span className={`text-xs font-semibold ${info.color}`}>{info.label}</span>
                  </>
                );
              })()}
            </div>
          )}
        </div>

        {/* Water Section */}
        <div>
          <p className="font-black text-gray-900 mb-3 flex items-center gap-2">
            <span className="text-xl">💧</span> Água
          </p>
          <Stepper
            value={agua}
            onChange={setAgua}
            label="Caixas d'água"
            sublabel="Referência: ~8 caixas/dia para este lote"
            icon="💧"
            variant="water"
          />
          {aguaStatus && (
            <div className={`mt-2 flex items-center gap-1.5 px-3 py-1.5 rounded-xl ${
              aguaStatus === "normal" ? "bg-green-50" : aguaStatus === "baixo" ? "bg-red-50" : "bg-amber-50"
            }`}>
              {(() => {
                const info = statusInfo[aguaStatus];
                const Icon = info.icon;
                return (
                  <>
                    <Icon size={14} className={info.color} />
                    <span className={`text-xs font-semibold ${info.color}`}>{info.label}</span>
                  </>
                );
              })()}
            </div>
          )}
        </div>

        {/* Summary */}
        {(racao > 0 || agua > 0) && (
          <div className="bg-white border-2 border-gray-100 rounded-2xl p-4">
            <p className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-3">Resumo do Apontamento</p>
            <div className="grid grid-cols-2 gap-3">
              <div className="bg-amber-50 rounded-xl p-3 text-center">
                <p className="text-3xl font-black text-amber-700">{racao}</p>
                <p className="text-xs text-amber-600 font-medium">🌾 Sacos de Ração</p>
              </div>
              <div className="bg-blue-50 rounded-xl p-3 text-center">
                <p className="text-3xl font-black text-blue-700">{agua}</p>
                <p className="text-xs text-blue-600 font-medium">💧 Caixas d'água</p>
              </div>
            </div>
          </div>
        )}

        {/* Tip */}
        <div className="bg-gray-100 rounded-2xl p-3 flex gap-2">
          <span className="text-base">ℹ️</span>
          <p className="text-xs text-gray-500">
            Registre o consumo <strong>após</strong> o fornecimento da última refeição do dia.
          </p>
        </div>
      </div>

      {/* Footer */}
      <div className="fixed bottom-16 left-0 right-0 max-w-md mx-auto p-4 bg-white border-t-2 border-gray-100">
        <button
          onClick={handleConfirm}
          disabled={racao === 0 && agua === 0}
          className="w-full h-16 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-3 text-lg active:scale-95 transition-all disabled:opacity-40 shadow-lg shadow-green-200"
        >
          <CheckCircle2 size={24} />
          Confirmar Consumo
        </button>
        {racao === 0 && agua === 0 && (
          <p className="text-center text-xs text-gray-400 mt-2">Registre ração ou água para confirmar</p>
        )}
      </div>
    </div>
  );
}