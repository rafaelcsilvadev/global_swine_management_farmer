import { useNavigate } from "react-router";
import { Baby, Skull, Wheat, Heart, CheckCircle, Clock, ChevronRight, User, CalendarDays } from "lucide-react";
import { useFarm } from "../context/FarmContext";

const statusConfig: Record<string, { label: string; color: string; icon: React.ElementType }> = {
  feito: { label: "Concluído", color: "text-green-600 bg-green-50", icon: CheckCircle },
  pendente: { label: "Pendente", color: "text-amber-600 bg-amber-50", icon: Clock },
};

const badgeStyles: Record<string, string> = {
  red: "bg-red-100 text-red-700",
  green: "bg-green-100 text-green-700",
  amber: "bg-amber-100 text-amber-700",
  default: "bg-gray-100 text-gray-600",
};

export default function HomeScreen() {
  const navigate = useNavigate();
  const { selection } = useFarm();
  const { galpao, lote, animal } = selection;

  const today = new Date().toLocaleDateString("pt-BR", {
    weekday: "long",
    day: "2-digit",
    month: "long",
  });

  const tasks = [
    {
      id: "maternidade",
      path: "/maternidade",
      icon: Baby,
      iconBg: "bg-pink-100",
      iconColor: "text-pink-600",
      title: "Registrar Partos",
      subtitle: `Maternidade — ${galpao.label}`,
      status: "pendente",
      badge: "Hoje",
    },
    {
      id: "mortalidade",
      path: "/checkup",
      icon: Skull,
      iconBg: "bg-red-100",
      iconColor: "text-red-600",
      title: "Apontar Mortalidade",
      subtitle: "Checkup diário de baixas",
      status: "pendente",
      badge: "Urgente",
      badgeColor: "red",
    },
    {
      id: "consumo",
      path: "/consumo",
      icon: Wheat,
      iconBg: "bg-amber-100",
      iconColor: "text-amber-600",
      title: "Consumo de Ração e Água",
      subtitle: `${lote.label} — ${galpao.type}`,
      status: "feito",
      badge: "Concluído",
      badgeColor: "green",
    },
    {
      id: "saude",
      path: "/saude",
      icon: Heart,
      iconBg: "bg-green-100",
      iconColor: "text-green-600",
      title: "Saúde e Medicação",
      subtitle: "Relatar sintomas e tratamentos",
      status: "pendente",
      badge: "Hoje",
    },
  ];

  const done = tasks.filter((t) => t.status === "feito").length;
  const total = tasks.length;
  const progress = (done / total) * 100;

  return (
    <div className="flex flex-col h-full">
      {/* Header */}
      <div className="bg-green-600 px-4 pt-4 pb-6">
        <div className="flex items-center gap-3 mb-4">
          <div className="w-11 h-11 rounded-full bg-white/20 flex items-center justify-center">
            <User size={22} className="text-white" />
          </div>
          <div>
            <p className="text-green-100 text-xs font-medium">Bom dia,</p>
            <p className="text-white font-black">João Silva</p>
          </div>
          <div className="ml-auto flex items-center gap-1 bg-white/10 rounded-xl px-3 py-1.5">
            <CalendarDays size={14} className="text-green-100" />
            <span className="text-green-100 text-xs capitalize">{today}</span>
          </div>
        </div>

        {/* Progress */}
        <div className="bg-white/10 rounded-2xl p-3">
          <div className="flex justify-between items-center mb-2">
            <span className="text-white font-semibold text-sm">Progresso do Dia</span>
            <span className="text-white font-black">{done}/{total}</span>
          </div>
          <div className="w-full bg-white/20 rounded-full h-3">
            <div
              className="bg-white rounded-full h-3 transition-all duration-500"
              style={{ width: `${progress}%` }}
            />
          </div>
          <p className="text-green-100 text-xs mt-1.5">
            {done === total
              ? "🎉 Todas as tarefas concluídas!"
              : `${total - done} tarefa${total - done !== 1 ? "s" : ""} restante${total - done !== 1 ? "s" : ""}`}
          </p>
        </div>
      </div>

      {/* Task List */}
      <div className="flex-1 overflow-y-auto px-4 py-4 pb-24 bg-gray-50">
        <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-3">
          Tarefas do Galpão
        </p>
        <div className="flex flex-col gap-3">
          {tasks.map((task) => {
            const Icon = task.icon;
            const StatusIcon = statusConfig[task.status].icon;
            const badgeColor = task.badgeColor ?? "default";
            const isDone = task.status === "feito";

            return (
              <button
                key={task.id}
                onClick={() => navigate(task.path)}
                className={`w-full flex items-center gap-4 bg-white rounded-2xl p-4 shadow-sm border-2 active:scale-98 transition-all text-left ${
                  isDone ? "border-green-100 opacity-75" : "border-gray-100"
                }`}
              >
                <div className={`w-14 h-14 rounded-2xl ${task.iconBg} flex items-center justify-center flex-shrink-0`}>
                  <Icon size={28} className={task.iconColor} strokeWidth={1.8} />
                </div>
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-0.5">
                    <p className={`font-bold truncate ${isDone ? "line-through text-gray-400" : "text-gray-900"}`}>
                      {task.title}
                    </p>
                  </div>
                  <p className="text-xs text-gray-500 truncate">{task.subtitle}</p>
                  <div className="flex items-center gap-1.5 mt-2">
                    <span className={`inline-flex items-center gap-1 text-xs font-semibold px-2 py-0.5 rounded-full ${statusConfig[task.status].color}`}>
                      <StatusIcon size={10} />
                      {statusConfig[task.status].label}
                    </span>
                    <span className={`text-xs font-bold px-2 py-0.5 rounded-full ${badgeStyles[badgeColor]}`}>
                      {task.badge}
                    </span>
                  </div>
                </div>
                <ChevronRight size={20} className="text-gray-300 flex-shrink-0" />
              </button>
            );
          })}
        </div>

        {/* Quick info */}
        <div className="mt-4 bg-blue-50 border-2 border-blue-100 rounded-2xl p-3 flex items-start gap-3">
          <span className="text-2xl">{galpao.emoji}</span>
          <div>
            <p className="text-sm font-bold text-blue-800">
              {lote.label} · {animal.label}
            </p>
            <p className="text-xs text-blue-600">
              {lote.animals} animais · {galpao.type} · {galpao.label} · Dia {lote.daysActive}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}