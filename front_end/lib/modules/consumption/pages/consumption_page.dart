import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import '../widgets/consumption_stepper_widget.dart';
import '../widgets/consumption_summary_card.dart';
import '../../home/home_routes.dart';
import '../../home/widgets/sync_status_bar.dart';
import '../../home/widgets/context_selector_bar.dart';

import '../../home/widgets/app_bottom_navigation_bar.dart';

class ConsumptionPage extends StatefulWidget {
  const ConsumptionPage({super.key});

  @override
  State<ConsumptionPage> createState() => _ConsumptionPageState();
}

class _ConsumptionPageState extends State<ConsumptionPage> {
  int _racao = 0;
  int _agua = 0;
  bool _saved = false;
  final int _selectedIndex = 2; // "Consumo" tab is selected

  final int _racaoRef = 3;
  final int _aguaRef = 8;

  String? _getRacaoStatus() {
    if (_racao == 0) return null;
    if (_racao < 2.5) return 'baixo';
    if (_racao > 3.5) return 'alto';
    return 'normal';
  }

  String? _getAguaStatus() {
    if (_agua == 0) return null;
    if (_agua < 7) return 'baixo';
    if (_agua > 9) return 'alto';
    return 'normal';
  }

  void _handleConfirm() {
    if (_racao > 0 || _agua > 0) {
      setState(() {
        _saved = true;
      });
      Future.delayed(const Duration(milliseconds: 1800), () {
        if (mounted) {
          context.go(HomeRoutes.home);
        }
      });
    }
  }

  Widget _buildStatusIndicator(String status) {
    late final Color bgColor;
    late final Color textColor;
    late final String label;
    late final IconData icon;

    switch (status) {
      case 'baixo':
        bgColor = const Color(0xFFFEF2F2); // Red-50
        textColor = const Color(0xFFDC2626); // Red-600
        label = 'Abaixo do normal';
        icon = Icons.trending_down;
        break;
      case 'alto':
        bgColor = const Color(0xFFFFF7ED); // Orange-50
        textColor = const Color(0xFFD97706); // Amber-600
        label = 'Acima do normal';
        icon = Icons.trending_up;
        break;
      case 'normal':
      default:
        bgColor = const Color(0xFFF0FDF4); // Green-50
        textColor = const Color(0xFF16A34A); // Green-600
        label = 'Consumo normal';
        icon = Icons.remove;
        break;
    }

    return Container(
      margin: const EdgeInsets.only(top: 8),
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
      decoration: BoxDecoration(
        color: bgColor,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(icon, size: 14, color: textColor),
          const SizedBox(width: 6),
          Text(
            label,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.bold,
              color: textColor,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeader(BuildContext context) {
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

          // Title & Subtitle
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: const [
                Text(
                  'Consumo de Insumos',
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w900,
                    color: Color(0xFF0F172A),
                  ),
                ),
                SizedBox(height: 2),
                Text(
                  'Registrar ração e água do lote',
                  style: TextStyle(
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

  Widget _buildBatchCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
        border: Border.all(
          color: const Color(0xFFF1F5F9),
          width: 2,
        ),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Emoji container
          Container(
            padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(
              color: const Color(0xFFFFF1F2), // Pink-50
              borderRadius: BorderRadius.circular(16),
            ),
            child: const Text(
              '🐷',
              style: TextStyle(fontSize: 28),
            ),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: const [
                Text(
                  'Lote 2024-047',
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.w900,
                    color: Color(0xFF0F172A),
                  ),
                ),
                SizedBox(height: 4),
                Text(
                  '842 animais · Terminação · Galpão B · Dia 87',
                  style: TextStyle(
                    fontSize: 12,
                    color: Color(0xFF64748B),
                    fontWeight: FontWeight.w600,
                  ),
                ),
                SizedBox(height: 2),
                Text(
                  'Grupo Completo · Todos os 842 animais do lote',
                  style: TextStyle(
                    fontSize: 11,
                    color: Color(0xFF94A3B8),
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

  Widget _buildTipCard() {
    return Container(
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: const Color(0xFFF1F5F9),
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text('ℹ️', style: TextStyle(fontSize: 16)),
          const SizedBox(width: 8),
          Expanded(
            child: RichText(
              text: const TextSpan(
                style: TextStyle(
                  fontSize: 12,
                  color: Color(0xFF64748B),
                  height: 1.4,
                ),
                children: [
                  TextSpan(text: 'Registre o consumo '),
                  TextSpan(
                    text: 'após',
                    style: TextStyle(fontWeight: FontWeight.bold, color: Color(0xFF334155)),
                  ),
                  TextSpan(text: ' o fornecimento da última refeição do dia.'),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFooterCta() {
    final bool isEnabled = _racao > 0 || _agua > 0;

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
                    'Confirmar Consumo',
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
              'Registre ração ou água para confirmar',
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

  Widget _buildSuccessState() {
    return Scaffold(
      backgroundColor: const Color(0xFFF0FDF4), // Green-50
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                width: 96,
                height: 96,
                decoration: const BoxDecoration(
                  color: Color(0xFFDCFCE7),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.check_circle,
                  color: Color(0xFF16A34A),
                  size: 48,
                ),
              ),
              const SizedBox(height: 24),
              const Text(
                'Consumo Registrado!',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.w900,
                  color: Color(0xFF0F172A),
                ),
              ),
              const SizedBox(height: 8),
              Text(
                'Ração: $_racao sacos · Água: $_agua caixas d\'água',
                textAlign: TextAlign.center,
                style: const TextStyle(
                  fontSize: 16,
                  color: Color(0xFF64748B),
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_saved) {
      return _buildSuccessState();
    }

    final String? racaoStatus = _getRacaoStatus();
    final String? aguaStatus = _getAguaStatus();

    return Scaffold(
      backgroundColor: const Color(0xFFF8F9FA),
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

            // Header
            _buildHeader(context),

            // Scrollable Content
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
                          // Batch Card
                          _buildBatchCard(),
                          const SizedBox(height: 20),

                          // Ração Section
                          const Text(
                            '🌾 Ração',
                            style: TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.w900,
                              color: Color(0xFF0F172A),
                            ),
                          ),
                          const SizedBox(height: 12),
                          ConsumptionStepperWidget(
                            value: _racao,
                            onChange: (val) => setState(() => _racao = val),
                            label: 'Sacos de Ração',
                            sublabel: 'Referência: ~$_racaoRef sacos/dia para este lote',
                            icon: '🌾',
                            variant: 'feed',
                          ),
                          if (racaoStatus != null) _buildStatusIndicator(racaoStatus),
                          const SizedBox(height: 24),

                          // Água Section
                          const Text(
                            '💧 Água',
                            style: TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.w900,
                              color: Color(0xFF0F172A),
                            ),
                          ),
                          const SizedBox(height: 12),
                          ConsumptionStepperWidget(
                            value: _agua,
                            onChange: (val) => setState(() => _agua = val),
                            label: 'Caixas d\'água',
                            sublabel: 'Referência: ~$_aguaRef caixas/dia para este lote',
                            icon: '💧',
                            variant: 'water',
                          ),
                          if (aguaStatus != null) _buildStatusIndicator(aguaStatus),
                          const SizedBox(height: 24),

                          // Summary Card
                          if (_racao > 0 || _agua > 0) ...[
                            ConsumptionSummaryCard(
                              racao: _racao,
                              agua: _agua,
                            ),
                            const SizedBox(height: 20),
                          ],

                          // Tip Card
                          _buildTipCard(),

                          // Spacing under content to allow scrolling past footer CTA
                          const SizedBox(height: 140),
                        ],
                      ),
                    ),
                  ),

                  // Fixed Footer CTA
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
}
