import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';
import '../widgets/stepper_widget.dart';
import '../widgets/birth_summary_card.dart';
import '../../home/home_routes.dart';
import '../../home/widgets/sync_status_bar.dart';
import '../../home/widgets/context_selector_bar.dart';
import '../../home/widgets/app_bottom_navigation_bar.dart';
import '../../home/view_models/farm_context_view_model.dart';

class BirthPage extends StatefulWidget {
  const BirthPage({super.key});

  @override
  State<BirthPage> createState() => _BirthPageState();
}

class _BirthPageState extends State<BirthPage> {
  int _vivos = 0;
  int _natimortos = 0;
  int _mumificados = 0;
  final int _selectedIndex = 1; // "Partos" tab is selected

  int get _total => _vivos + _natimortos + _mumificados;

  void _handleConfirm() {
    if (_total > 0) {
      context.go('/birth/success/$_total');
    }
  }

  @override
  Widget build(BuildContext context) {
    final farmContext = context.watch<FarmContextViewModel>();

    return Scaffold(
      backgroundColor: const Color(0xFFF8F9FA), // Slate-50 background color
      appBar: AppBar(
        toolbarHeight: 0,
        backgroundColor: const Color(0xFF00A63E),
        elevation: 0,
      ),
      body: SafeArea(
        child: Column(
          children: [
            // Sync status bar
            const SyncStatusBar(
              isOnline: true,
              pendingItems: 0,
              isSyncing: false,
            ),

            // Context selector bar
            const ContextSelectorBar(),

            // Custom header
            _buildHeader(context, farmContext),

            // Main scrollable content
            Expanded(
              child: Stack(
                children: [
                  SingleChildScrollView(
                    physics: const BouncingScrollPhysics(),
                    child: Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          // Pink Info Card
                          _buildPinkInfoCard(farmContext),
                          const SizedBox(height: 16),

                          // Separator line "Use os botões abaixo"
                          _buildSeparator(),
                          const SizedBox(height: 16),

                          // Nascidos Vivos Stepper
                          StepperWidget(
                            value: _vivos,
                            onChange: (val) => setState(() => _vivos = val),
                            label: 'Nascidos Vivos',
                            sublabel: 'Leitões que nasceram vivos e saudáveis',
                            icon: '🐷',
                            variant: 'default',
                          ),
                          const SizedBox(height: 12),

                          // Natimortos Stepper
                          StepperWidget(
                            value: _natimortos,
                            onChange: (val) => setState(() => _natimortos = val),
                            label: 'Natimortos',
                            sublabel: 'Nascidos sem vida durante o parto',
                            icon: '💀',
                            variant: 'alert',
                          ),
                          const SizedBox(height: 12),

                          // Mumificados Stepper
                          StepperWidget(
                            value: _mumificados,
                            onChange: (val) => setState(() => _mumificados = val),
                            label: 'Mumificados',
                            sublabel: 'Fetos mumificados encontrados',
                            icon: '🔴',
                            variant: 'alert',
                          ),
                          const SizedBox(height: 16),

                          // Summary Card
                          if (_total > 0) ...[
                            BirthSummaryCard(
                              vivos: _vivos,
                              natimortos: _natimortos,
                              mumificados: _mumificados,
                            ),
                          ],

                          // Bottom spacing to prevent scrolling under the CTA container
                          const SizedBox(height: 130),
                        ],
                      ),
                    ),
                  ),

                  // Fixed Footer CTA at the bottom of the content column
                  Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: _buildFooterCta(),
                  ),
                ],
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

  Widget _buildHeader(BuildContext context, FarmContextViewModel farmContext) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      decoration: const BoxDecoration(
        color: Colors.white,
        border: Border(
          bottom: BorderSide(
            color: Color(0xFFEEEEEE),
            width: 1,
          ),
        ),
      ),
      child: Row(
        children: [
          // Back button
          GestureDetector(
            onTap: () {
              if (Navigator.canPop(context)) {
                Navigator.pop(context);
              } else {
                context.go(HomeRoutes.home);
              }
            },
            child: Container(
              padding: const EdgeInsets.all(10),
              decoration: BoxDecoration(
                color: const Color(0xFFF1F5F9),
                borderRadius: BorderRadius.circular(14),
              ),
              child: const Icon(
                Icons.arrow_back,
                color: Color(0xFF475569),
                size: 20,
              ),
            ),
          ),
          const SizedBox(width: 12),

          // Title & Subtitle & Chip
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    const Text(
                      'Registro de Partos',
                      style: TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.w900,
                        color: Color(0xFF0F172A),
                      ),
                    ),
                    const SizedBox(width: 8),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                      decoration: BoxDecoration(
                        color: const Color(0xFFDBEAFE), // Light blue badge
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: Text(
                        farmContext.selectedLote['label'] ?? 'Lote 2024-047',
                        style: const TextStyle(
                          fontSize: 10,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF1D4ED8), // Dark blue text
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 2),
                Text(
                  '${farmContext.selectedGalpao['label']} · ${farmContext.selectedGalpao['type']}',
                  style: const TextStyle(
                    fontSize: 12,
                    color: Color(0xFF64748B),
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPinkInfoCard(FarmContextViewModel farmContext) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFFFFF1F2), // Pink-50
        borderRadius: BorderRadius.circular(20),
        border: Border.all(
          color: const Color(0xFFFCE7F3), // Pink-100
          width: 2,
        ),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            padding: const EdgeInsets.all(8),
            decoration: const BoxDecoration(
              color: Color(0xFFFCE7F3),
              shape: BoxShape.circle,
            ),
            child: const Icon(
              Icons.child_care,
              color: Color(0xFFD6006B),
              size: 24,
            ),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  farmContext.selectedLote['label'] ?? 'Lote 2024-047',
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.w900,
                    color: Color(0xFF9D174D), // Pink-800
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  farmContext.selectedAnimal['detail'] ?? 'Todos os animais do lote',
                  style: const TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    color: Color(0xFFBE185D), // Pink-700
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSeparator() {
    return Row(
      children: const [
        Expanded(child: Divider(color: Color(0xFFE2E8F0))),
        Padding(
          padding: EdgeInsets.symmetric(horizontal: 10),
          child: Text(
            'Use os botões abaixo',
            style: TextStyle(
              fontSize: 12,
              color: Color(0xFF94A3B8),
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
        Expanded(child: Divider(color: Color(0xFFE2E8F0))),
      ],
    );
  }

  Widget _buildFooterCta() {
    final bool isEnabled = _total > 0;

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: const BoxDecoration(
        color: Colors.white,
        border: Border(
          top: BorderSide(
            color: Color(0xFFF1F5F9),
            width: 2,
          ),
        ),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          SizedBox(
            width: double.infinity,
            height: 56,
            child: ElevatedButton(
              onPressed: isEnabled ? _handleConfirm : null,
              style: ElevatedButton.styleFrom(
                backgroundColor: const Color(0xFF16A34A),
                disabledBackgroundColor: const Color(0xFF16A34A).withValues(alpha: 0.4),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
                elevation: 0,
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: const [
                  Icon(
                    Icons.check_circle_outline,
                    color: Colors.white,
                    size: 24,
                  ),
                  SizedBox(width: 8),
                  Text(
                    'Confirmar Parto',
                    style: TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.w900,
                      color: Colors.white,
                    ),
                  ),
                ],
              ),
            ),
          ),
          if (!isEnabled) ...[
            const SizedBox(height: 8),
            const Text(
              'Registre ao menos 1 leitão para confirmar',
              style: TextStyle(
                fontSize: 12,
                color: Color(0xFF94A3B8),
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ],
      ),
    );
  }
}
