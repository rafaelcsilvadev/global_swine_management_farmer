import { createContext, useContext, useState, ReactNode } from "react";

// ─── Data Types ───────────────────────────────────────────────
export interface Galpao {
  id: string;
  label: string;
  type: string;
  emoji: string;
  color: string;
  bg: string;
  count: number;
}

export interface Lote {
  id: string;
  label: string;
  detail: string;
  animals: number;
  daysActive: number;
}

export interface Animal {
  id: string;
  label: string;
  detail: string;
  tag?: string;
}

// ─── Mock Data ────────────────────────────────────────────────
export const GALPOES: Galpao[] = [
  { id: "A", label: "Galpão A", type: "Maternidade", emoji: "🍼", color: "text-pink-700", bg: "bg-pink-50", count: 48 },
  { id: "B", label: "Galpão B", type: "Terminação",  emoji: "🐷", color: "text-amber-700", bg: "bg-amber-50", count: 842 },
  { id: "C", label: "Galpão C", type: "Crescimento", emoji: "🌱", color: "text-green-700", bg: "bg-green-50", count: 560 },
  { id: "D", label: "Galpão D", type: "Gestação",    emoji: "🤰", color: "text-purple-700", bg: "bg-purple-50", count: 120 },
];

export const LOTES: Record<string, Lote[]> = {
  A: [
    { id: "MAT-012", label: "Lote MAT-012", detail: "3 dias de vida · 11 porcas", animals: 11, daysActive: 3 },
    { id: "MAT-011", label: "Lote MAT-011", detail: "8 dias de vida · 14 porcas", animals: 14, daysActive: 8 },
    { id: "MAT-010", label: "Lote MAT-010", detail: "15 dias de vida · 9 porcas", animals: 9, daysActive: 15 },
  ],
  B: [
    { id: "2024-047", label: "Lote 2024-047", detail: "Dia 87 de terminação · 842 animais", animals: 842, daysActive: 87 },
    { id: "2024-046", label: "Lote 2024-046", detail: "Dia 102 de terminação · 790 animais", animals: 790, daysActive: 102 },
    { id: "2024-045", label: "Lote 2024-045", detail: "Dia 118 de terminação · 610 animais", animals: 610, daysActive: 118 },
  ],
  C: [
    { id: "2024-051", label: "Lote 2024-051", detail: "Dia 42 de crescimento · 560 animais", animals: 560, daysActive: 42 },
    { id: "2024-050", label: "Lote 2024-050", detail: "Dia 58 de crescimento · 510 animais", animals: 510, daysActive: 58 },
  ],
  D: [
    { id: "GES-023", label: "Lote GES-023", detail: "Dia 35 de gestação · 120 porcas", animals: 120, daysActive: 35 },
    { id: "GES-022", label: "Lote GES-022", detail: "Dia 50 de gestação · 98 porcas", animals: 98, daysActive: 50 },
  ],
};

export const ANIMAIS: Record<string, Animal[]> = {
  "MAT-012": [
    { id: "grupo", label: "Grupo Completo", detail: "Todas as 11 porcas do lote", tag: "👥" },
    { id: "P041", label: "Porca #041", detail: "2ª Leitegada · Em trabalho de parto", tag: "🔴" },
    { id: "P042", label: "Porca #042", detail: "1ª Leitegada · Recém-parida", tag: "🟡" },
    { id: "P043", label: "Porca #043", detail: "3ª Leitegada · Amamentando", tag: "🟢" },
    { id: "P044", label: "Porca #044", detail: "2ª Leitegada · Amamentando", tag: "🟢" },
    { id: "P045", label: "Porca #045", detail: "1ª Leitegada · Aguardando parto", tag: "🔵" },
  ],
  "MAT-011": [
    { id: "grupo", label: "Grupo Completo", detail: "Todas as 14 porcas do lote", tag: "👥" },
    { id: "P031", label: "Porca #031", detail: "2ª Leitegada · Amamentando", tag: "🟢" },
    { id: "P032", label: "Porca #032", detail: "1ª Leitegada · Amamentando", tag: "🟢" },
    { id: "P033", label: "Porca #033", detail: "3ª Leitegada · Com sintomas", tag: "🔴" },
  ],
  "MAT-010": [
    { id: "grupo", label: "Grupo Completo", detail: "Todas as 9 porcas do lote", tag: "👥" },
    { id: "P021", label: "Porca #021", detail: "4ª Leitegada · Desmame próximo", tag: "🟡" },
    { id: "P022", label: "Porca #022", detail: "2ª Leitegada · Amamentando", tag: "🟢" },
  ],
};

// For non-maternidade lotes, return a single "grupo" option
const DEFAULT_ANIMAIS = (loteId: string, count: number): Animal[] => [
  { id: "grupo", label: "Grupo Completo", detail: `Todos os ${count} animais do lote`, tag: "👥" },
];

export function getAnimais(galpaoId: string, loteId: string): Animal[] {
  if (galpaoId === "A" && ANIMAIS[loteId]) return ANIMAIS[loteId];
  const lote = LOTES[galpaoId]?.find((l) => l.id === loteId);
  return DEFAULT_ANIMAIS(loteId, lote?.animals ?? 0);
}

// ─── Context ──────────────────────────────────────────────────
export interface FarmSelection {
  galpao: Galpao;
  lote: Lote;
  animal: Animal;
}

interface FarmContextType {
  selection: FarmSelection;
  setSelection: (s: FarmSelection) => void;
  openSelector: () => void;
  closeSelector: () => void;
  isSelectorOpen: boolean;
}

const FarmContext = createContext<FarmContextType | null>(null);

const DEFAULT_SELECTION: FarmSelection = {
  galpao: GALPOES[1],
  lote: LOTES["B"][0],
  animal: { id: "grupo", label: "Grupo Completo", detail: "Todos os 842 animais do lote", tag: "👥" },
};

export function FarmProvider({ children }: { children: ReactNode }) {
  const [selection, setSelection] = useState<FarmSelection>(DEFAULT_SELECTION);
  const [isSelectorOpen, setIsSelectorOpen] = useState(false);

  return (
    <FarmContext.Provider
      value={{
        selection,
        setSelection,
        openSelector: () => setIsSelectorOpen(true),
        closeSelector: () => setIsSelectorOpen(false),
        isSelectorOpen,
      }}
    >
      {children}
    </FarmContext.Provider>
  );
}

export function useFarm() {
  const ctx = useContext(FarmContext);
  if (!ctx) throw new Error("useFarm must be used inside FarmProvider");
  return ctx;
}
