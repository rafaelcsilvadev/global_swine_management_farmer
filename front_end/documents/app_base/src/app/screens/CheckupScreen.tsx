import { useState } from "react";
import { useNavigate } from "react-router";
import { Camera, CheckCircle2, ThumbsUp, ThumbsDown, Minus, AlertTriangle } from "lucide-react";
import { ScreenHeader } from "../components/ScreenHeader";
import { Stepper } from "../components/Stepper";
import { useFarm } from "../context/FarmContext";

const symptomTags = [
  { id: "tosse", label: "Tosse", emoji: "🤧" },
  { id: "diarreia", label: "Diarreia", emoji: "💧" },
  { id: "prostrado", label: "Prostrado", emoji: "😴" },
  { id: "manqueira", label: "Manqueira", emoji: "🦵" },
  { id: "tremor", label: "Tremores", emoji: "〰️" },
  { id: "vomito", label: "Vômito", emoji: "🤢" },
  { id: "cianose", label: "Cianose", emoji: "🔵" },
  { id: "inapetencia", label: "Inapetência", emoji: "🚫" },
];

type EfficacyType = "melhorou" | "igual" | "piorou" | null;

export default function CheckupScreen() {
  const navigate = useNavigate();
  const { selection } = useFarm();
  const { galpao, lote, animal } = selection;
  const [mortes, setMortes] = useState(0);
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [efficacy, setEfficacy] = useState<EfficacyType>(null);
  const [photoCaptured, setPhotoCaptured] = useState(false);
  const [saved, setSaved] = useState(false);

  const toggleSymptom = (id: string) => {
    setSelectedSymptoms((prev) =>
      prev.includes(id) ? prev.filter((s) => s !== id) : [...prev, id]
    );
  };

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
        <h2 className="font-black text-gray-900">Checkup Enviado!</h2>
        <p className="text-gray-500 text-sm">
          {mortes > 0
            ? `${mortes} baixa${mortes !== 1 ? "s" : ""} registrada${mortes !== 1 ? "s" : ""}.`
            : "Sem baixas registradas hoje."}{" "}
          Dados enviados para análise.
        </p>
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full">
      <ScreenHeader
        title="Checkup Diário"
        subtitle="Saúde e Mortalidade do Lote"
        badge={lote.label}
        badgeColor="amber"
      />

      <div className="flex-1 overflow-y-auto px-4 py-4 pb-36 bg-gray-50 flex flex-col gap-5">
        {/* Context info */}
        <div className={`flex items-center gap-2 px-3 py-2 rounded-xl ${galpao.bg}`}>
          <span className="text-lg">{galpao.emoji}</span>
          <span className={`text-xs font-bold ${galpao.color}`}>
            {galpao.label} · {galpao.type}
          </span>
          <span className="text-gray-400 mx-1">·</span>
          <span className="text-xs font-semibold text-gray-600">{animal.label}</span>
        </div>

        {/* Mortality Section */}
        <div>
          <div className="flex items-center gap-2 mb-3">
            <AlertTriangle size={18} className="text-red-500" />
            <p className="font-black text-gray-900">Mortalidade (Baixas)</p>
          </div>
          <Stepper
            value={mortes}
            onChange={setMortes}
            label="Animais Mortos"
            sublabel="Número de baixas encontradas hoje"
            icon="💀"
            variant="alert"
          />
          {mortes > 0 && (
            <div className="mt-2 bg-red-50 border border-red-200 rounded-xl px-3 py-2 flex items-center gap-2">
              <AlertTriangle size={14} className="text-red-500 flex-shrink-0" />
              <p className="text-xs text-red-600 font-semibold">
                {mortes} baixa{mortes !== 1 ? "s" : ""} detectada{mortes !== 1 ? "s" : ""}. Notificação automática ao veterinário.
              </p>
            </div>
          )}
        </div>

        {/* Divider */}
        <div className="flex items-center gap-2">
          <div className="flex-1 h-px bg-gray-200" />
          <span className="text-xs text-gray-400 font-medium">Sintomas Observados</span>
          <div className="flex-1 h-px bg-gray-200" />
        </div>

        {/* Symptom Tags */}
        <div>
          <p className="text-xs text-gray-400 mb-3">Toque nos sintomas que você identificou no lote:</p>
          <div className="flex flex-wrap gap-2">
            {symptomTags.map((s) => {
              const active = selectedSymptoms.includes(s.id);
              return (
                <button
                  key={s.id}
                  onClick={() => toggleSymptom(s.id)}
                  className={`flex items-center gap-1.5 px-3 py-2 rounded-xl border-2 transition-all active:scale-95 ${
                    active
                      ? "bg-red-50 border-red-400 text-red-700"
                      : "bg-white border-gray-200 text-gray-600"
                  }`}
                >
                  <span className="text-base">{s.emoji}</span>
                  <span className="text-sm font-semibold">{s.label}</span>
                </button>
              );
            })}
          </div>
        </div>

        {/* Divider */}
        <div className="flex items-center gap-2">
          <div className="flex-1 h-px bg-gray-200" />
          <span className="text-xs text-gray-400 font-medium">Eficácia do Tratamento</span>
          <div className="flex-1 h-px bg-gray-200" />
        </div>

        {/* Treatment Efficacy */}
        <div>
          <p className="text-xs text-gray-500 mb-2 font-medium">Os sintomas do último tratamento regrediram?</p>
          <div className="bg-amber-50 border border-amber-100 rounded-xl px-3 py-2 mb-3 flex gap-2 items-center">
            <span className="text-base">💉</span>
            <span className="text-xs text-amber-700 font-medium">Amoxicilina 20mg · há 3 dias</span>
          </div>
          <div className="grid grid-cols-3 gap-2">
            {[
              { id: "melhorou", label: "Melhorou", icon: ThumbsUp, colorActive: "bg-green-50 border-green-500 text-green-700", colorInactive: "bg-white border-gray-200 text-gray-500" },
              { id: "igual", label: "Sem mudança", icon: Minus, colorActive: "bg-amber-50 border-amber-500 text-amber-700", colorInactive: "bg-white border-gray-200 text-gray-500" },
              { id: "piorou", label: "Piorou", icon: ThumbsDown, colorActive: "bg-red-50 border-red-500 text-red-700", colorInactive: "bg-white border-gray-200 text-gray-500" },
            ].map(({ id, label, icon: Icon, colorActive, colorInactive }) => {
              const active = efficacy === id;
              return (
                <button
                  key={id}
                  onClick={() => setEfficacy(id as EfficacyType)}
                  className={`flex flex-col items-center gap-2 py-4 rounded-2xl border-2 transition-all active:scale-95 ${active ? colorActive : colorInactive}`}
                >
                  <Icon size={22} strokeWidth={2} />
                  <span className="text-xs font-bold text-center leading-tight">{label}</span>
                </button>
              );
            })}
          </div>
        </div>

        {/* Camera Section */}
        <div>
          <p className="font-black text-gray-900 mb-1">📷 Foto/Vídeo do Lote</p>
          <p className="text-xs text-gray-400 mb-3">
            <span className="text-red-500 font-bold">Obrigatório</span> — Necessário para triagem remota preventiva pelo veterinário
          </p>
          <button
            onClick={() => setPhotoCaptured(true)}
            className={`w-full h-20 rounded-2xl flex items-center justify-center gap-4 transition-all active:scale-95 border-2 border-dashed ${
              photoCaptured
                ? "bg-green-50 border-green-400 text-green-700"
                : "bg-gray-100 border-gray-300 text-gray-600"
            }`}
          >
            {photoCaptured ? (
              <>
                <CheckCircle2 size={28} className="text-green-600" />
                <div className="text-left">
                  <p className="font-black text-green-800">Mídia Capturada!</p>
                  <p className="text-xs text-green-600">Toque para gravar novamente</p>
                </div>
              </>
            ) : (
              <>
                <div className="w-12 h-12 bg-gray-200 rounded-xl flex items-center justify-center">
                  <Camera size={24} className="text-gray-600" />
                </div>
                <div className="text-left">
                  <p className="font-black text-gray-800">Tirar Foto ou Gravar Vídeo</p>
                  <p className="text-xs text-gray-500">Toque para abrir a câmera</p>
                </div>
              </>
            )}
          </button>
        </div>
      </div>

      {/* Footer */}
      <div className="fixed bottom-16 left-0 right-0 max-w-md mx-auto p-4 bg-white border-t-2 border-gray-100">
        <button
          onClick={handleConfirm}
          className="w-full h-16 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-3 text-lg active:scale-95 transition-all shadow-lg shadow-green-200"
        >
          <CheckCircle2 size={24} />
          Confirmar Checkup
        </button>
      </div>
    </div>
  );
}