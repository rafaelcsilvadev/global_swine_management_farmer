import { useState } from "react";
import { useNavigate } from "react-router";
import { Camera, CheckCircle2, ThumbsUp, ThumbsDown, Minus } from "lucide-react";
import { ScreenHeader } from "../components/ScreenHeader";
import { useFarm } from "../context/FarmContext";

const symptoms = [
  { id: "tosse", label: "Tosse", emoji: "🤧" },
  { id: "diarreia", label: "Diarreia", emoji: "💧" },
  { id: "prostrado", label: "Prostrado", emoji: "😴" },
  { id: "manqueira", label: "Manqueira", emoji: "🦵" },
  { id: "tremor", label: "Tremores", emoji: "〰️" },
  { id: "vomito", label: "Vômito", emoji: "🤢" },
  { id: "cianose", label: "Cianose", emoji: "🔵" },
  { id: "edema", label: "Edema", emoji: "🫁" },
  { id: "inapetencia", label: "Inapetência", emoji: "🚫" },
  { id: "febre", label: "Febre", emoji: "🌡️" },
  { id: "lesoes", label: "Lesões/Feridas", emoji: "🩹" },
  { id: "dispneia", label: "Dispneia", emoji: "😮‍💨" },
];

type EfficacyType = "melhorou" | "igual" | "piorou" | null;

export default function SaudeScreen() {
  const navigate = useNavigate();
  const { selection } = useFarm();
  const { galpao, lote, animal } = selection;
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [efficacy, setEfficacy] = useState<EfficacyType>(null);
  const [photoCaptured, setPhotoCaptured] = useState(false);
  const [saved, setSaved] = useState(false);

  const toggleSymptom = (id: string) => {
    setSelectedSymptoms((prev) =>
      prev.includes(id) ? prev.filter((s) => s !== id) : [...prev, id]
    );
  };

  const handlePhoto = () => {
    setPhotoCaptured(true);
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
        <h2 className="font-black text-gray-900">Relatório Enviado!</h2>
        <p className="text-gray-500 text-sm">O veterinário receberá os dados para análise remota.</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full">
      <ScreenHeader
        title="Saúde e Medicação"
        subtitle={`${galpao.label} · ${lote.label}`}
        badge="Telemedicina"
        badgeColor="blue"
      />

      <div className="flex-1 overflow-y-auto px-4 py-4 pb-36 bg-gray-50 flex flex-col gap-5">
        {/* Symptoms Section */}
        <div>
          <div className="flex items-center gap-2 mb-3">
            <p className="font-black text-gray-900">Sintomas Observados</p>
            {selectedSymptoms.length > 0 && (
              <span className="bg-red-100 text-red-700 text-xs font-bold px-2 py-0.5 rounded-full">
                {selectedSymptoms.length} selecionado{selectedSymptoms.length !== 1 ? "s" : ""}
              </span>
            )}
          </div>
          <p className="text-xs text-gray-400 mb-3">Toque nos sintomas que você observou no lote:</p>
          <div className="grid grid-cols-3 gap-2">
            {symptoms.map((s) => {
              const active = selectedSymptoms.includes(s.id);
              return (
                <button
                  key={s.id}
                  onClick={() => toggleSymptom(s.id)}
                  className={`flex flex-col items-center justify-center gap-1.5 p-3 rounded-2xl border-2 transition-all active:scale-95 ${
                    active
                      ? "bg-red-50 border-red-400 shadow-sm"
                      : "bg-white border-gray-200"
                  }`}
                >
                  <span className="text-2xl">{s.emoji}</span>
                  <span className={`text-xs font-semibold text-center leading-tight ${active ? "text-red-700" : "text-gray-600"}`}>
                    {s.label}
                  </span>
                </button>
              );
            })}
          </div>
        </div>

        {/* Camera Button */}
        <div>
          <p className="font-black text-gray-900 mb-2">Foto/Vídeo do Lote</p>
          <p className="text-xs text-gray-400 mb-3">Necessário para triagem remota pelo veterinário</p>
          <button
            onClick={handlePhoto}
            className={`w-full h-20 rounded-2xl flex items-center justify-center gap-4 transition-all active:scale-95 border-2 border-dashed ${
              photoCaptured
                ? "bg-green-50 border-green-400 text-green-700"
                : "bg-blue-50 border-blue-300 text-blue-700"
            }`}
          >
            {photoCaptured ? (
              <>
                <CheckCircle2 size={28} className="text-green-600" />
                <div className="text-left">
                  <p className="font-black">Foto Capturada!</p>
                  <p className="text-xs text-green-600">Toque para tirar outra</p>
                </div>
              </>
            ) : (
              <>
                <div className="w-12 h-12 bg-blue-100 rounded-xl flex items-center justify-center">
                  <Camera size={26} className="text-blue-600" />
                </div>
                <div className="text-left">
                  <p className="font-black">Capturar Foto ou Vídeo</p>
                  <p className="text-xs text-blue-500">
                    {animal.id === "grupo" ? lote.label : animal.label} · {animal.detail}
                  </p>
                </div>
              </>
            )}
          </button>
        </div>

        {/* Treatment Efficacy */}
        <div>
          <p className="font-black text-gray-900 mb-1">Eficácia do Tratamento Anterior</p>
          <p className="text-xs text-gray-400 mb-3">Os sintomas do último tratamento melhoraram?</p>
          <div className="grid grid-cols-3 gap-2">
            {[
              { id: "melhorou", label: "Melhorou", icon: ThumbsUp, color: "green" },
              { id: "igual", label: "Sem mudança", icon: Minus, color: "amber" },
              { id: "piorou", label: "Piorou", icon: ThumbsDown, color: "red" },
            ].map(({ id, label, icon: Icon, color }) => {
              const active = efficacy === id;
              const colorMap: Record<string, string> = {
                green: active ? "bg-green-50 border-green-500 text-green-700" : "bg-white border-gray-200 text-gray-600",
                amber: active ? "bg-amber-50 border-amber-500 text-amber-700" : "bg-white border-gray-200 text-gray-600",
                red: active ? "bg-red-50 border-red-500 text-red-700" : "bg-white border-gray-200 text-gray-600",
              };
              return (
                <button
                  key={id}
                  onClick={() => setEfficacy(id as EfficacyType)}
                  className={`flex flex-col items-center gap-2 py-4 rounded-2xl border-2 transition-all active:scale-95 ${colorMap[color]}`}
                >
                  <Icon size={24} strokeWidth={2} />
                  <span className="text-xs font-bold text-center">{label}</span>
                </button>
              );
            })}
          </div>
        </div>

        {/* Last Treatment Note */}
        <div className="bg-amber-50 border-2 border-amber-100 rounded-2xl p-3 flex gap-3">
          <span className="text-xl">💉</span>
          <div>
            <p className="text-sm font-bold text-amber-800">Último Tratamento</p>
            <p className="text-xs text-amber-600">Amoxicilina 20mg · Aplicado há 3 dias por João</p>
          </div>
        </div>
      </div>

      {/* Footer */}
      <div className="fixed bottom-16 left-0 right-0 max-w-md mx-auto p-4 bg-white border-t-2 border-gray-100">
        <button
          onClick={handleConfirm}
          disabled={selectedSymptoms.length === 0 && !photoCaptured}
          className="w-full h-16 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-3 text-lg active:scale-95 transition-all disabled:opacity-40 shadow-lg shadow-green-200"
        >
          <CheckCircle2 size={24} />
          Enviar Relatório
        </button>
      </div>
    </div>
  );
}