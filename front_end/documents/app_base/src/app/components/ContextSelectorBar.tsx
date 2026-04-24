import { Pencil, MapPin, Package, ChevronRight } from "lucide-react";
import { useFarm } from "../context/FarmContext";

export function ContextSelectorBar() {
  const { selection, openSelector } = useFarm();
  const { galpao, lote, animal } = selection;

  return (
    <button
      onClick={openSelector}
      className="w-full flex items-center gap-2 px-3 py-2.5 bg-white border-b-2 border-gray-100 active:bg-gray-50 transition-all"
    >
      {/* Galpão pill */}
      <div className={`flex items-center gap-1.5 px-2.5 py-1 rounded-xl ${galpao.bg} flex-shrink-0`}>
        <span className="text-base leading-none">{galpao.emoji}</span>
        <span className={`text-xs font-black ${galpao.color}`}>{galpao.label}</span>
      </div>

      <ChevronRight size={12} className="text-gray-300 flex-shrink-0" />

      {/* Lote pill */}
      <div className="flex items-center gap-1 px-2.5 py-1 rounded-xl bg-gray-100 flex-shrink-0">
        <Package size={11} className="text-gray-500" />
        <span className="text-xs font-bold text-gray-600 truncate max-w-[72px]">{lote.label}</span>
      </div>

      <ChevronRight size={12} className="text-gray-300 flex-shrink-0" />

      {/* Animal pill */}
      <div className="flex items-center gap-1 min-w-0 flex-1">
        <span className="text-sm leading-none flex-shrink-0">{animal.tag}</span>
        <span className="text-xs font-semibold text-gray-600 truncate">{animal.label}</span>
      </div>

      {/* Edit button */}
      <div className="ml-auto flex-shrink-0 flex items-center gap-1 bg-green-50 border border-green-200 rounded-xl px-2.5 py-1">
        <Pencil size={11} className="text-green-600" />
        <span className="text-xs font-bold text-green-700">Alterar</span>
      </div>
    </button>
  );
}
