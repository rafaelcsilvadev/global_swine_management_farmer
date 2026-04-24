import { useState } from "react";
import { useNavigate } from "react-router";
import { CheckCircle2, Baby } from "lucide-react";
import { ScreenHeader } from "../components/ScreenHeader";
import { Stepper } from "../components/Stepper";
import { useFarm } from "../context/FarmContext";

export default function MaternidadeScreen() {
  const navigate = useNavigate();
  const { selection } = useFarm();
  const { galpao, lote, animal } = selection;
  const [vivos, setVivos] = useState(0);
  const [natimortos, setNatimortos] = useState(0);
  const [mumificados, setMumificados] = useState(0);
  const [saved, setSaved] = useState(false);

  const total = vivos + natimortos + mumificados;

  const handleConfirm = () => {
    setSaved(true);
    setTimeout(() => {
      navigate("/");
    }, 1800);
  };

  if (saved) {
    return (
      <div className="flex flex-col items-center justify-center h-full bg-green-50 px-6 text-center gap-4">
        <div className="w-24 h-24 bg-green-100 rounded-full flex items-center justify-center">
          <CheckCircle2 size={48} className="text-green-600" />
        </div>
        <h2 className="font-black text-gray-900">Parto Registrado!</h2>
        <p className="text-gray-500 text-sm">
          {total} leitão{total !== 1 ? "es" : ""} registrado{total !== 1 ? "s" : ""} com sucesso.
        </p>
        <p className="text-xs text-gray-400">Redirecionando...</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full">
      <ScreenHeader
        title="Registro de Partos"
        subtitle={`${galpao.label} · ${galpao.type}`}
        badge={animal.id === "grupo" ? lote.label : animal.label}
        badgeColor="blue"
      />

      <div className="flex-1 overflow-y-auto px-4 py-4 pb-36 bg-gray-50 flex flex-col gap-4">
        {/* Info Card */}
        <div className="bg-pink-50 border-2 border-pink-100 rounded-2xl p-3 flex items-center gap-3">
          <Baby size={28} className="text-pink-500 flex-shrink-0" />
          <div>
            <p className="font-bold text-pink-800 text-sm">
              {animal.id === "grupo" ? lote.label : animal.label}
            </p>
            <p className="text-xs text-pink-600">{animal.detail}</p>
          </div>
        </div>

        {/* Instructions */}
        <div className="flex items-center gap-2">
          <div className="flex-1 h-px bg-gray-200" />
          <span className="text-xs text-gray-400 font-medium">Use os botões abaixo</span>
          <div className="flex-1 h-px bg-gray-200" />
        </div>

        {/* Steppers */}
        <Stepper
          value={vivos}
          onChange={setVivos}
          label="Nascidos Vivos"
          sublabel="Leitões que nasceram vivos e saudáveis"
          icon="🐷"
          variant="default"
        />

        <Stepper
          value={natimortos}
          onChange={setNatimortos}
          label="Natimortos"
          sublabel="Nascidos sem vida durante o parto"
          icon="💀"
          variant="alert"
        />

        <Stepper
          value={mumificados}
          onChange={setMumificados}
          label="Mumificados"
          sublabel="Fetos mumificados encontrados"
          icon="🔴"
          variant="alert"
        />

        {/* Summary */}
        {total > 0 && (
          <div className="bg-white border-2 border-gray-100 rounded-2xl p-4">
            <p className="text-xs font-bold text-gray-400 uppercase tracking-wider mb-3">Resumo</p>
            <div className="grid grid-cols-3 gap-2 text-center">
              <div className="bg-green-50 rounded-xl p-2">
                <p className="text-2xl font-black text-green-700">{vivos}</p>
                <p className="text-xs text-green-600 font-medium">Vivos</p>
              </div>
              <div className="bg-red-50 rounded-xl p-2">
                <p className="text-2xl font-black text-red-700">{natimortos}</p>
                <p className="text-xs text-red-600 font-medium">Natimortos</p>
              </div>
              <div className="bg-red-50 rounded-xl p-2">
                <p className="text-2xl font-black text-red-700">{mumificados}</p>
                <p className="text-xs text-red-600 font-medium">Mumificados</p>
              </div>
            </div>
            <div className="mt-3 pt-3 border-t border-gray-100 text-center">
              <p className="text-xs text-gray-500">Total nascidos: <span className="font-black text-gray-900">{total}</span></p>
            </div>
          </div>
        )}
      </div>

      {/* Footer CTA */}
      <div className="fixed bottom-16 left-0 right-0 max-w-md mx-auto p-4 bg-white border-t-2 border-gray-100">
        <button
          onClick={handleConfirm}
          disabled={total === 0}
          className="w-full h-16 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-3 text-lg active:scale-95 transition-all disabled:opacity-40 disabled:cursor-not-allowed shadow-lg shadow-green-200"
        >
          <CheckCircle2 size={24} />
          Confirmar Parto
        </button>
        {total === 0 && (
          <p className="text-center text-xs text-gray-400 mt-2">Registre ao menos 1 leitão para confirmar</p>
        )}
      </div>
    </div>
  );
}