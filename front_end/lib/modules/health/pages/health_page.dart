import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import '../widgets/symptom_card.dart';
import '../../home/home_routes.dart';
import '../../home/widgets/sync_status_bar.dart';
import '../../home/widgets/context_selector_bar.dart';

import '../../home/widgets/app_bottom_navigation_bar.dart';

class HealthPage extends StatefulWidget {
  const HealthPage({super.key});

  @override
  State<HealthPage> createState() => _HealthPageState();
}

class _HealthPageState extends State<HealthPage> {
  final int _selectedIndex = 3; // "Saúde" tab is selected
  final List<String> _selectedSymptoms = [];
  bool _photoCaptured = false;
  String? _efficacy; // 'melhorou', 'igual', 'piorou'

  final List<Map<String, String>> _symptoms = const [
    {'id': 'tosse', 'label': 'Tosse', 'emoji': '🤧'},
    {'id': 'diarreia', 'label': 'Diarreia', 'emoji': '💧'},
    {'id': 'prostrado', 'label': 'Prostrado', 'emoji': '😴'},
    {'id': 'manqueira', 'label': 'Manqueira', 'emoji': '🦵'},
    {'id': 'tremor', 'label': 'Tremores', 'emoji': '〰️'},
    {'id': 'vomito', 'label': 'Vômito', 'emoji': '🤢'},
    {'id': 'cianose', 'label': 'Cianose', 'emoji': '🔵'},
    {'id': 'edema', 'label': 'Edema', 'emoji': '🫁'},
    {'id': 'inapetencia', 'label': 'Inapetência', 'emoji': '🚫'},
    {'id': 'febre', 'label': 'Febre', 'emoji': '🌡️'},
    {'id': 'lesoes', 'label': 'Lesões/Feridas', 'emoji': '🩹'},
    {'id': 'dispneia', 'label': 'Dispneia', 'emoji': '😮‍💨'},
  ];

  void _toggleSymptom(String id) {
    setState(() {
      if (_selectedSymptoms.contains(id)) {
        _selectedSymptoms.remove(id);
      } else {
        _selectedSymptoms.add(id);
      }
    });
  }

  void _handleConfirm() {
    if (_selectedSymptoms.isNotEmpty || _photoCaptured) {
      context.go(
        '/health/success',
        extra: {
          'symptoms': List<String>.from(_selectedSymptoms),
          'photoCaptured': _photoCaptured,
        },
      );
    }
  }

  @override
  Widget build(BuildContext context) {
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
            _buildHeader(context),

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
                          // Symptoms observed section header
                          _buildSectionHeader(),
                          const SizedBox(height: 12),

                          // Symptoms Grid
                          GridView.builder(
                            shrinkWrap: true,
                            physics: const NeverScrollableScrollPhysics(),
                            itemCount: _symptoms.length,
                            gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                              crossAxisCount: 3,
                              mainAxisSpacing: 8,
                              crossAxisSpacing: 8,
                              childAspectRatio: 0.95,
                            ),
                            itemBuilder: (context, index) {
                              final item = _symptoms[index];
                              final isSelected = _selectedSymptoms.contains(item['id']!);
                              return SymptomCard(
                                label: item['label']!,
                                emoji: item['emoji']!,
                                isSelected: isSelected,
                                onTap: () => _toggleSymptom(item['id']!),
                              );
                            },
                          ),
                          const SizedBox(height: 24),

                          // Camera Button Section
                          _buildCameraSection(),
                          const SizedBox(height: 24),

                          // Treatment Efficacy Section
                          _buildEfficacySection(),
                          const SizedBox(height: 20),

                          // Last Treatment Note
                          _buildLastTreatmentNote(),

                          // Bottom spacing to prevent scrolling under the CTA container
                          const SizedBox(height: 130),
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

          // Title & Badge & Subtitle
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    const Text(
                      'Saúde e Medicação',
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
                      child: const Text(
                        'Telemedicina',
                        style: TextStyle(
                          fontSize: 10,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF1D4ED8), // Dark blue text
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 2),
                const Text(
                  'Galpão B · Lote 2024-047',
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

  Widget _buildSectionHeader() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            const Text(
              'Sintomas Observados',
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.w900,
                color: Color(0xFF0F172A),
              ),
            ),
            if (_selectedSymptoms.isNotEmpty) ...[
              const SizedBox(width: 8),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                decoration: BoxDecoration(
                  color: const Color(0xFFFEE2E2), // Red-100
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Text(
                  '${_selectedSymptoms.length} selecionado${_selectedSymptoms.length != 1 ? 's' : ''}',
                  style: const TextStyle(
                    fontSize: 10,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFFB91C1C), // Red-700
                  ),
                ),
              ),
            ],
          ],
        ),
        const SizedBox(height: 2),
        const Text(
          'Toque nos sintomas que você observou no lote:',
          style: TextStyle(
            fontSize: 12,
            color: Color(0xFF94A3B8),
            fontWeight: FontWeight.w500,
          ),
        ),
      ],
    );
  }

  Widget _buildCameraSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Foto/Vídeo do Lote',
          style: TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.w900,
            color: Color(0xFF0F172A),
          ),
        ),
        const SizedBox(height: 2),
        const Text(
          'Necessário para triagem remota pelo veterinário',
          style: TextStyle(
            fontSize: 12,
            color: Color(0xFF94A3B8),
            fontWeight: FontWeight.w500,
          ),
        ),
        const SizedBox(height: 12),
        InkWell(
          onTap: () {
            setState(() {
              _photoCaptured = !_photoCaptured;
            });
          },
          borderRadius: BorderRadius.circular(20),
          child: AnimatedContainer(
            duration: const Duration(milliseconds: 200),
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 16),
            decoration: BoxDecoration(
              color: _photoCaptured ? const Color(0xFFF0FDF4) : const Color(0xFFEFF6FF),
              borderRadius: BorderRadius.circular(20),
              border: Border.all(
                color: _photoCaptured ? const Color(0xFF86EFAC) : const Color(0xFF93C5FD),
                width: 2,
              ),
            ),
            child: Row(
              children: [
                Container(
                  width: 48,
                  height: 48,
                  decoration: BoxDecoration(
                    color: _photoCaptured ? const Color(0xFFDCFCE7) : const Color(0xFFDBEAFE),
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Icon(
                    _photoCaptured ? Icons.check_circle : Icons.camera_alt,
                    color: _photoCaptured ? const Color(0xFF16A34A) : const Color(0xFF2563EB),
                    size: 26,
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        _photoCaptured ? 'Foto Capturada!' : 'Capturar Foto ou Vídeo',
                        style: TextStyle(
                          fontSize: 15,
                          fontWeight: FontWeight.w900,
                          color: _photoCaptured ? const Color(0xFF14532D) : const Color(0xFF1E3A8A),
                        ),
                      ),
                      const SizedBox(height: 2),
                      Text(
                        _photoCaptured
                            ? 'Toque para tirar outra'
                            : 'Lote 2024-047 · Grupo Completo',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.w500,
                          color: _photoCaptured ? const Color(0xFF16A34A) : const Color(0xFF3B82F6),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildEfficacySection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Eficácia do Tratamento Anterior',
          style: TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.w900,
            color: Color(0xFF0F172A),
          ),
        ),
        const SizedBox(height: 2),
        const Text(
          'Os sintomas do último tratamento melhoraram?',
          style: TextStyle(
            fontSize: 12,
            color: Color(0xFF94A3B8),
            fontWeight: FontWeight.w500,
          ),
        ),
        const SizedBox(height: 12),
        Row(
          children: [
            _buildEfficacyBtn('melhorou', 'Melhorou', Icons.thumb_up_alt_outlined, 'green'),
            const SizedBox(width: 8),
            _buildEfficacyBtn('igual', 'Sem mudança', Icons.remove, 'amber'),
            const SizedBox(width: 8),
            _buildEfficacyBtn('piorou', 'Piorou', Icons.thumb_down_alt_outlined, 'red'),
          ],
        ),
      ],
    );
  }

  Widget _buildEfficacyBtn(String id, String label, IconData icon, String colorKey) {
    final bool active = _efficacy == id;

    Color activeBg;
    Color activeBorder;
    Color activeText;

    if (colorKey == 'green') {
      activeBg = const Color(0xFFF0FDF4); // Green-50
      activeBorder = const Color(0xFF22C55E); // Green-500
      activeText = const Color(0xFF15803D); // Green-700
    } else if (colorKey == 'amber') {
      activeBg = const Color(0xFFFFFBEB); // Amber-50
      activeBorder = const Color(0xFFF59E0B); // Amber-500
      activeText = const Color(0xFFB45309); // Amber-700
    } else {
      activeBg = const Color(0xFFFEF2F2); // Red-50
      activeBorder = const Color(0xFFEF4444); // Red-500
      activeText = const Color(0xFFB91C1C); // Red-700
    }

    final Color bgColor = active ? activeBg : Colors.white;
    final Color borderColor = active ? activeBorder : const Color(0xFFE2E8F0);
    final Color textColor = active ? activeText : const Color(0xFF475569);

    return Expanded(
      child: InkWell(
        onTap: () {
          setState(() {
            _efficacy = active ? null : id;
          });
        },
        borderRadius: BorderRadius.circular(20),
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 150),
          padding: const EdgeInsets.symmetric(vertical: 14),
          decoration: BoxDecoration(
            color: bgColor,
            borderRadius: BorderRadius.circular(20),
            border: Border.all(
              color: borderColor,
              width: 2,
            ),
          ),
          child: Column(
            children: [
              Icon(icon, color: textColor, size: 24),
              const SizedBox(height: 6),
              Text(
                label,
                style: TextStyle(
                  fontSize: 11,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildLastTreatmentNote() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFFFFFBEB), // Amber-50
        borderRadius: BorderRadius.circular(20),
        border: Border.all(
          color: const Color(0xFFFEF3C7), // Amber-100
          width: 2,
        ),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            '💉',
            style: TextStyle(fontSize: 24),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: const [
                Text(
                  'Último Tratamento',
                  style: TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.w900,
                    color: Color(0xFF92400E), // Amber-800
                  ),
                ),
                SizedBox(height: 2),
                Text(
                  'Amoxicilina 20mg · Aplicado há 3 dias por João',
                  style: TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    color: Color(0xFFD97706), // Amber-600
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFooterCta() {
    final bool isEnabled = _selectedSymptoms.isNotEmpty || _photoCaptured;

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
                    'Enviar Relatório',
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
              'Selecione um sintoma ou capture foto para enviar',
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
