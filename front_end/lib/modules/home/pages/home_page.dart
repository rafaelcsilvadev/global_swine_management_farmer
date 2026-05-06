import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';
import '../widgets/sync_status_bar.dart';
import '../widgets/context_selector_bar.dart';
import '../widgets/home_header.dart';
import '../widgets/task_card.dart';
import '../widgets/batch_info_card.dart';
import '../widgets/app_bottom_navigation_bar.dart';
import '../view_models/farm_context_view_model.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final int _selectedIndex = 0;

  @override
  Widget build(BuildContext context) {
    final farmContext = context.watch<FarmContextViewModel>();

    return Scaffold(
      backgroundColor: const Color(0xFFF8F9FA), // Slightly lighter gray (gray-50)
      appBar: AppBar(
        toolbarHeight: 0,
        backgroundColor: const Color(0xFF00A63E),
        elevation: 0,
      ),
      body: SafeArea(
        child: Column(
          children: [
            const SyncStatusBar(
              isOnline: true,
              pendingItems: 0,
              isSyncing: false,
            ),
            const ContextSelectorBar(),
            Expanded(
              child: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const HomeHeader(
                      userName: 'João Silva',
                      completedTasks: 1,
                      totalTasks: 4,
                    ),
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'TAREFAS DO GALPÃO',
                            style: TextStyle(
                              fontSize: 11,
                              fontWeight: FontWeight.w900,
                              color: Colors.grey.shade400,
                              letterSpacing: 1.2,
                            ),
                          ),
                          const SizedBox(height: 12),
                          TaskCard(
                            title: 'Registrar Partos',
                            subtitle: 'Maternidade — Galpão B',
                            icon: Icons.child_care,
                            iconColor: const Color(0xFFD6006B),
                            iconBackgroundColor: const Color(0xFFFDEEF7),
                            status: 'Pendente',
                            statusColor: const Color(0xFFEA580C),
                            statusBackgroundColor: const Color(0xFFFFF7ED),
                            dateText: 'Hoje',
                            onTap: () => context.go('/birth'),
                          ),

                          const SizedBox(height: 12),
                          TaskCard(
                            title: 'Consumo de Ração e Água',
                            subtitle: 'Lote 2024-047 — Terminação',
                            icon: Icons.eco,
                            iconColor: const Color(0xFFD97706),
                            iconBackgroundColor: const Color(0xFFFEF3C7),
                            status: 'Concluído',
                            statusColor: const Color(0xFF16A34A),
                            statusBackgroundColor: const Color(0xFFDCFCE7),
                            statusIcon: Icons.check_circle_outline,
                            dateText: 'Concluído',
                            dateColor: const Color(0xFF16A34A),
                            dateBackgroundColor: const Color(0xFFDCFCE7),
                            isCompleted: true,
                            onTap: () => context.go('/consumption'),
                          ),
                          const SizedBox(height: 12),
                          TaskCard(
                            title: 'Saúde e Medicação',
                            subtitle: 'Relatar sintomas e tratamentos',
                            icon: Icons.favorite_border,
                            iconColor: const Color(0xFF16A34A),
                            iconBackgroundColor: const Color(0xFFDCFCE7),
                            status: 'Pendente',
                            statusColor: const Color(0xFFEA580C),
                            statusBackgroundColor: const Color(0xFFFFF7ED),
                            dateText: 'Hoje',
                            onTap: () => context.go('/health'),
                          ),
                          const SizedBox(height: 16),
                          BatchInfoCard(
                            batchName: farmContext.selectedLote['label'] ?? 'Lote 2024-047',
                            groupName: farmContext.selectedAnimal['label'] ?? 'Grupo Completo',
                            animalCount: farmContext.selectedLote['animals'] ?? 842,
                            phase: farmContext.selectedGalpao['type'] ?? 'Terminação',
                            shed: farmContext.selectedGalpao['label'] ?? 'Galpão B',
                            day: farmContext.selectedLote['daysActive'] ?? 87,
                          ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
          ],
        ),
      ),
      bottomNavigationBar: AppBottomNavigationBar(
        selectedIndex: _selectedIndex,
      ),
    );
  }
}

