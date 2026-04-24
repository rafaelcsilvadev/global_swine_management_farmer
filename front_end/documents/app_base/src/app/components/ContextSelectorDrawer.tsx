import { useState, useEffect } from "react";
import { X, ChevronRight, ChevronLeft, Check, MapPin, Package, Rabbit } from "lucide-react";
import {
  useFarm,
  GALPOES,
  LOTES,
  getAnimais,
  type Galpao,
  type Lote,
  type Animal,
  type FarmSelection,
} from "../context/FarmContext";

type Step = 1 | 2 | 3;

const stepConfig = [
  { step: 1 as Step, label: "Galpão", icon: MapPin },
  { step: 2 as Step, label: "Lote",   icon: Package },
  { step: 3 as Step, label: "Animal", icon: Rabbit },
];

export function ContextSelectorDrawer() {
  const { isSelectorOpen, closeSelector, selection, setSelection } = useFarm();

  const [step, setStep] = useState<Step>(1);
  const [tmpGalpao, setTmpGalpao] = useState<Galpao>(selection.galpao);
  const [tmpLote, setTmpLote]     = useState<Lote>(selection.lote);
  const [tmpAnimal, setTmpAnimal] = useState<Animal>(selection.animal);
  const [visible, setVisible]     = useState(false);

  // Animate in/out
  useEffect(() => {
    if (isSelectorOpen) {
      // Reset to current selection
      setTmpGalpao(selection.galpao);
      setTmpLote(selection.lote);
      setTmpAnimal(selection.animal);
      setStep(1);
      requestAnimationFrame(() => setVisible(true));
    } else {
      setVisible(false);
    }
  }, [isSelectorOpen]);

  const handleClose = () => {
    setVisible(false);
    setTimeout(closeSelector, 280);
  };

  const handleSelectGalpao = (g: Galpao) => {
    setTmpGalpao(g);
    const firstLote = LOTES[g.id][0];
    setTmpLote(firstLote);
    setTmpAnimal(getAnimais(g.id, firstLote.id)[0]);
    setTimeout(() => setStep(2), 120);
  };

  const handleSelectLote = (l: Lote) => {
    setTmpLote(l);
    setTmpAnimal(getAnimais(tmpGalpao.id, l.id)[0]);
    setTimeout(() => setStep(3), 120);
  };

  const handleSelectAnimal = (a: Animal) => {
    setTmpAnimal(a);
  };

  const handleConfirm = () => {
    const newSelection: FarmSelection = {
      galpao: tmpGalpao,
      lote: tmpLote,
      animal: tmpAnimal,
    };
    setSelection(newSelection);
    handleClose();
  };

  if (!isSelectorOpen) return null;

  const currentAnimais = getAnimais(tmpGalpao.id, tmpLote.id);

  return (
    <div className="fixed inset-0 z-50 flex flex-col justify-end max-w-md mx-auto">
      {/* Backdrop */}
      <div
        onClick={handleClose}
        className="absolute inset-0 bg-black/50 transition-opacity duration-300"
        style={{ opacity: visible ? 1 : 0 }}
      />

      {/* Drawer */}
      <div
        className="relative bg-white rounded-t-3xl flex flex-col transition-transform duration-300 ease-out max-h-[88vh]"
        style={{ transform: visible ? "translateY(0)" : "translateY(100%)" }}
      >
        {/* Handle */}
        <div className="flex justify-center pt-3 pb-1">
          <div className="w-10 h-1 bg-gray-200 rounded-full" />
        </div>

        {/* Header */}
        <div className="flex items-center justify-between px-5 py-3 border-b border-gray-100">
          <div>
            <p className="font-black text-gray-900">Alterar Contexto</p>
            <p className="text-xs text-gray-400">Selecione o local e o animal a vistoriar</p>
          </div>
          <button
            onClick={handleClose}
            className="w-9 h-9 rounded-full bg-gray-100 flex items-center justify-center active:bg-gray-200"
          >
            <X size={18} className="text-gray-600" />
          </button>
        </div>

        {/* Step Indicator */}
        <div className="flex items-center px-5 pt-4 pb-2 gap-1">
          {stepConfig.map(({ step: s, label, icon: Icon }) => {
            const isActive = step === s;
            const isDone   = step > s;
            return (
              <button
                key={s}
                onClick={() => s < step && setStep(s)}
                disabled={s > step}
                className="flex-1 flex flex-col items-center gap-1"
              >
                <div
                  className={`w-full h-1.5 rounded-full transition-all ${
                    isActive ? "bg-green-500" : isDone ? "bg-green-400" : "bg-gray-200"
                  }`}
                />
                <div className="flex items-center gap-1">
                  {isDone ? (
                    <Check size={10} className="text-green-500" />
                  ) : (
                    <Icon size={10} className={isActive ? "text-green-600" : "text-gray-400"} />
                  )}
                  <span
                    className={`text-xs font-semibold ${
                      isActive ? "text-green-700" : isDone ? "text-green-500" : "text-gray-400"
                    }`}
                  >
                    {label}
                  </span>
                </div>
              </button>
            );
          })}
        </div>

        {/* Content */}
        <div className="flex-1 overflow-y-auto px-4 py-3 flex flex-col gap-2">

          {/* ── STEP 1: Galpão ── */}
          {step === 1 && (
            <>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">
                Qual galpão você vai vistoriar?
              </p>
              {GALPOES.map((g) => {
                const isSelected = tmpGalpao.id === g.id;
                return (
                  <button
                    key={g.id}
                    onClick={() => handleSelectGalpao(g)}
                    className={`w-full flex items-center gap-4 p-4 rounded-2xl border-2 transition-all active:scale-98 text-left ${
                      isSelected
                        ? "border-green-500 bg-green-50"
                        : "border-gray-150 bg-gray-50"
                    }`}
                  >
                    <div className={`w-14 h-14 rounded-2xl ${g.bg} flex items-center justify-center flex-shrink-0`}>
                      <span className="text-3xl">{g.emoji}</span>
                    </div>
                    <div className="flex-1">
                      <p className={`font-black ${isSelected ? "text-green-800" : "text-gray-900"}`}>
                        {g.label}
                      </p>
                      <p className={`text-sm ${isSelected ? "text-green-600" : "text-gray-500"}`}>
                        {g.type}
                      </p>
                      <p className="text-xs text-gray-400 mt-0.5">{g.count} animais</p>
                    </div>
                    {isSelected ? (
                      <div className="w-7 h-7 rounded-full bg-green-500 flex items-center justify-center flex-shrink-0">
                        <Check size={14} className="text-white" />
                      </div>
                    ) : (
                      <ChevronRight size={20} className="text-gray-300 flex-shrink-0" />
                    )}
                  </button>
                );
              })}
            </>
          )}

          {/* ── STEP 2: Lote ── */}
          {step === 2 && (
            <>
              <button
                onClick={() => setStep(1)}
                className="flex items-center gap-1.5 text-green-600 mb-1 active:opacity-70"
              >
                <ChevronLeft size={16} />
                <span className="text-sm font-semibold">Voltar para Galpão</span>
              </button>
              <div className={`flex items-center gap-2 px-3 py-2 rounded-xl ${tmpGalpao.bg} mb-2`}>
                <span className="text-lg">{tmpGalpao.emoji}</span>
                <span className={`text-sm font-bold ${tmpGalpao.color}`}>
                  {tmpGalpao.label} · {tmpGalpao.type}
                </span>
              </div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">
                Qual lote você vai vistoriar?
              </p>
              {(LOTES[tmpGalpao.id] ?? []).map((l) => {
                const isSelected = tmpLote.id === l.id;
                return (
                  <button
                    key={l.id}
                    onClick={() => handleSelectLote(l)}
                    className={`w-full flex items-center gap-4 p-4 rounded-2xl border-2 transition-all active:scale-98 text-left ${
                      isSelected
                        ? "border-green-500 bg-green-50"
                        : "border-gray-150 bg-gray-50"
                    }`}
                  >
                    <div className="w-12 h-12 rounded-xl bg-white border-2 border-gray-200 flex items-center justify-center flex-shrink-0">
                      <Package size={22} className={isSelected ? "text-green-600" : "text-gray-400"} />
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className={`font-black ${isSelected ? "text-green-800" : "text-gray-900"}`}>
                        {l.label}
                      </p>
                      <p className="text-xs text-gray-500 mt-0.5">{l.detail}</p>
                      <p className="text-xs text-gray-400 mt-0.5">Dia {l.daysActive}</p>
                    </div>
                    {isSelected ? (
                      <div className="w-7 h-7 rounded-full bg-green-500 flex items-center justify-center flex-shrink-0">
                        <Check size={14} className="text-white" />
                      </div>
                    ) : (
                      <ChevronRight size={20} className="text-gray-300 flex-shrink-0" />
                    )}
                  </button>
                );
              })}
            </>
          )}

          {/* ── STEP 3: Animal ── */}
          {step === 3 && (
            <>
              <button
                onClick={() => setStep(2)}
                className="flex items-center gap-1.5 text-green-600 mb-1 active:opacity-70"
              >
                <ChevronLeft size={16} />
                <span className="text-sm font-semibold">Voltar para Lote</span>
              </button>
              <div className="flex items-center gap-2 mb-1">
                <div className={`flex items-center gap-1.5 px-3 py-1.5 rounded-xl ${tmpGalpao.bg}`}>
                  <span className="text-base">{tmpGalpao.emoji}</span>
                  <span className={`text-xs font-bold ${tmpGalpao.color}`}>{tmpGalpao.label}</span>
                </div>
                <ChevronRight size={14} className="text-gray-300" />
                <div className="flex items-center gap-1.5 px-3 py-1.5 rounded-xl bg-gray-100">
                  <Package size={13} className="text-gray-500" />
                  <span className="text-xs font-bold text-gray-600">{tmpLote.label}</span>
                </div>
              </div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">
                Qual animal ou grupo?
              </p>
              {currentAnimais.map((a) => {
                const isSelected = tmpAnimal.id === a.id;
                return (
                  <button
                    key={a.id}
                    onClick={() => handleSelectAnimal(a)}
                    className={`w-full flex items-center gap-4 p-4 rounded-2xl border-2 transition-all active:scale-98 text-left ${
                      isSelected
                        ? "border-green-500 bg-green-50"
                        : "border-gray-150 bg-gray-50"
                    }`}
                  >
                    <div className={`w-12 h-12 rounded-xl flex items-center justify-center flex-shrink-0 text-2xl ${
                      isSelected ? "bg-green-100" : "bg-white border-2 border-gray-200"
                    }`}>
                      {a.tag}
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className={`font-black ${isSelected ? "text-green-800" : "text-gray-900"}`}>
                        {a.label}
                      </p>
                      <p className="text-xs text-gray-500 mt-0.5">{a.detail}</p>
                    </div>
                    {isSelected && (
                      <div className="w-7 h-7 rounded-full bg-green-500 flex items-center justify-center flex-shrink-0">
                        <Check size={14} className="text-white" />
                      </div>
                    )}
                  </button>
                );
              })}
            </>
          )}
        </div>

        {/* Footer */}
        <div className="p-4 border-t border-gray-100">
          {step < 3 ? (
            <button
              onClick={() => setStep((s) => (s + 1) as Step)}
              className="w-full h-14 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-2 active:scale-95 transition-all"
            >
              Próximo
              <ChevronRight size={20} />
            </button>
          ) : (
            <button
              onClick={handleConfirm}
              className="w-full h-14 bg-green-600 text-white font-black rounded-2xl flex items-center justify-center gap-2 active:scale-95 transition-all shadow-lg shadow-green-200"
            >
              <Check size={22} />
              Confirmar Seleção
            </button>
          )}
        </div>
      </div>
    </div>
  );
}
