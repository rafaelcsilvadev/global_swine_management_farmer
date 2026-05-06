import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../../../shared/widgets/custom_button.dart';
import '../view_models/farm_context_view_model.dart';

class ContextSelectorDrawer extends StatefulWidget {
  const ContextSelectorDrawer({super.key});

  static Future<void> show(BuildContext context) {
    return showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (context) => const ContextSelectorDrawer(),
    );
  }

  @override
  State<ContextSelectorDrawer> createState() => _ContextSelectorDrawerState();
}

class _ContextSelectorDrawerState extends State<ContextSelectorDrawer> {
  int _currentStep = 1;

  Map<String, dynamic>? _selectedGalpao;
  Map<String, dynamic>? _selectedLote;
  Map<String, dynamic>? _selectedAnimal;

  @override
  void initState() {
    super.initState();
    final farmContext = Provider.of<FarmContextViewModel>(context, listen: false);

    _selectedGalpao = farmContext.galpoes.firstWhere(
      (g) => g['id'] == farmContext.selectedGalpao['id'],
      orElse: () => farmContext.galpoes[1],
    );

    final lotesList = farmContext.lotes[_selectedGalpao!['id']] ?? [];
    _selectedLote = lotesList.cast<Map<String, dynamic>?>().firstWhere(
      (l) => l?['id'] == farmContext.selectedLote['id'],
      orElse: () => lotesList.isNotEmpty ? lotesList[0] : null,
    );

    final animaisList = _selectedLote != null ? (farmContext.animais[_selectedLote!['id']] ?? []) : [];
    _selectedAnimal = animaisList.cast<Map<String, dynamic>?>().firstWhere(
      (a) => a?['id'] == farmContext.selectedAnimal['id'],
      orElse: () => animaisList.isNotEmpty ? animaisList[0] : null,
    );
  }

  void _nextStep() {
    if (_currentStep < 3) {
      setState(() => _currentStep++);
    } else {
      Provider.of<FarmContextViewModel>(context, listen: false).selectContext(
        galpao: _selectedGalpao!,
        lote: _selectedLote!,
        animal: _selectedAnimal!,
      );
      Navigator.pop(context);
    }
  }

  void _prevStep() {
    if (_currentStep > 1) {
      setState(() => _currentStep--);
    }
  }

  @override
  Widget build(BuildContext context) {
    final farmContext = context.watch<FarmContextViewModel>();

    return DraggableScrollableSheet(
      initialChildSize: 0.85,
      minChildSize: 0.5,
      maxChildSize: 0.95,
      builder: (_, scrollController) {
        return Container(
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(24)),
          ),
          child: Column(
            children: [
              // Handle
              Center(
                child: Container(
                  margin: const EdgeInsets.symmetric(vertical: 12),
                  width: 40,
                  height: 4,
                  decoration: BoxDecoration(
                    color: Colors.grey[300],
                    borderRadius: BorderRadius.circular(2),
                  ),
                ),
              ),

              // Header
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 8),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'Alterar Contexto',
                          style: TextStyle(
                            fontSize: 18,
                            fontWeight: FontWeight.w900,
                            color: Color(0xFF1A1A1A),
                          ),
                        ),
                        Text(
                          'Selecione o local e o animal a vistoriar',
                          style: TextStyle(
                            fontSize: 12,
                            color: Colors.grey[500],
                          ),
                        ),
                      ],
                    ),
                    IconButton(
                      onPressed: () => Navigator.pop(context),
                      icon: Container(
                        padding: const EdgeInsets.all(8),
                        decoration: BoxDecoration(
                          color: Colors.grey[100],
                          shape: BoxShape.circle,
                        ),
                        child: const Icon(Icons.close, size: 18, color: Color(0xFF616161)),
                      ),
                    ),
                  ],
                ),
              ),

              const Divider(height: 1),

              // Step Indicator
              Padding(
                padding: const EdgeInsets.fromLTRB(20, 16, 20, 8),
                child: Row(
                  children: [
                    _buildStepIndicator(1, 'Galpão', Icons.location_on_outlined),
                    const SizedBox(width: 8),
                    _buildStepIndicator(2, 'Lote', Icons.inventory_2_outlined),
                    const SizedBox(width: 8),
                    _buildStepIndicator(3, 'Animal', Icons.pets_outlined),
                  ],
                ),
              ),

              // Content
              Expanded(
                child: ListView(
                  controller: scrollController,
                  padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                  children: [
                    if (_currentStep == 1) ..._buildGalpaoList(farmContext),
                    if (_currentStep == 2) ..._buildLoteList(farmContext),
                    if (_currentStep == 3) ..._buildAnimalList(farmContext),
                  ],
                ),
              ),

              // Footer
              Padding(
                padding: const EdgeInsets.all(16),
                child: CustomButton(
                  text: _currentStep < 3 ? 'Próximo' : 'Confirmar Seleção',
                  onPressed: _nextStep,
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildStepIndicator(int step, String label, IconData icon) {
    bool isActive = _currentStep == step;
    bool isDone = _currentStep > step;

    return Expanded(
      child: Column(
        children: [
          Container(
            height: 4,
            decoration: BoxDecoration(
              color: isActive
                  ? const Color(0xFF00A63E)
                  : isDone
                      ? const Color(0xFF00A63E).withValues(alpha: 0.5)
                      : Colors.grey[200],
              borderRadius: BorderRadius.circular(2),
            ),
          ),
          const SizedBox(height: 8),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              if (isDone)
                const Icon(Icons.check, size: 12, color: Color(0xFF00A63E))
              else
                Icon(icon,
                    size: 12,
                    color: isActive ? const Color(0xFF00A63E) : Colors.grey[400]),
              const SizedBox(width: 4),
              Text(
                label,
                style: TextStyle(
                  fontSize: 11,
                  fontWeight: FontWeight.bold,
                  color: isActive
                      ? const Color(0xFF00A63E)
                      : isDone
                          ? const Color(0xFF00A63E).withValues(alpha: 0.7)
                          : Colors.grey[400],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  List<Widget> _buildGalpaoList(FarmContextViewModel farmContext) {
    return [
      const Text(
        'QUAL GALPÃO VOCÊ VAI VISTORIAR?',
        style: TextStyle(
          fontSize: 10,
          fontWeight: FontWeight.w900,
          color: Colors.grey,
          letterSpacing: 1.2,
        ),
      ),
      const SizedBox(height: 12),
      ...farmContext.galpoes.map((g) {
        bool isSelected = _selectedGalpao?['id'] == g['id'];
        return _buildSelectCard(
          isSelected: isSelected,
          onTap: () {
            setState(() {
              _selectedGalpao = g;
              final lotesList = farmContext.lotes[g['id']] ?? [];
              _selectedLote = lotesList.isNotEmpty ? lotesList[0] : null;
              if (_selectedLote != null) {
                final animaisList = farmContext.animais[_selectedLote!['id']] ?? [];
                _selectedAnimal = animaisList.isNotEmpty ? animaisList[0] : null;
              } else {
                _selectedAnimal = null;
              }
            });
            Future.delayed(const Duration(milliseconds: 200), _nextStep);
          },
          leading: Container(
            width: 56,
            height: 56,
            decoration: BoxDecoration(
              color: Color(g['bg'] as int),
              borderRadius: BorderRadius.circular(16),
            ),
            child: Center(
              child: Text(g['emoji'], style: const TextStyle(fontSize: 28)),
            ),
          ),
          title: g['label'],
          subtitle: g['type'],
          trailingText: '${g['count']} animais',
        );
      }),
    ];
  }

  List<Widget> _buildLoteList(FarmContextViewModel farmContext) {
    final lotesList = farmContext.lotes[_selectedGalpao?['id']] ?? [];

    return [
      GestureDetector(
        onTap: _prevStep,
        child: const Row(
          children: [
            Icon(Icons.chevron_left, size: 18, color: Color(0xFF00A63E)),
            Text(
              'Voltar para Galpão',
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: Color(0xFF00A63E),
              ),
            ),
          ],
        ),
      ),
      const SizedBox(height: 12),
      Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          color: Color(_selectedGalpao?['bg'] as int),
          borderRadius: BorderRadius.circular(12),
        ),
        child: Row(
          children: [
            Text(_selectedGalpao?['emoji'] ?? '', style: const TextStyle(fontSize: 20)),
            const SizedBox(width: 8),
            Text(
              '${_selectedGalpao?['label']} · ${_selectedGalpao?['type']}',
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: Color(_selectedGalpao?['color'] as int),
              ),
            ),
          ],
        ),
      ),
      const SizedBox(height: 16),
      const Text(
        'QUAL LOTE VOCÊ VAI VISTORIAR?',
        style: TextStyle(
          fontSize: 10,
          fontWeight: FontWeight.w900,
          color: Colors.grey,
          letterSpacing: 1.2,
        ),
      ),
      const SizedBox(height: 12),
      if (lotesList.isEmpty)
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 24),
          child: Center(
            child: Text(
              'Nenhum lote disponível neste galpão.',
              style: TextStyle(color: Colors.grey[600], fontSize: 13),
            ),
          ),
        )
      else
        ...lotesList.map((l) {
          bool isSelected = _selectedLote?['id'] == l['id'];
          return _buildSelectCard(
            isSelected: isSelected,
            onTap: () {
              setState(() {
                _selectedLote = l;
                final animaisList = farmContext.animais[l['id']] ?? [];
                _selectedAnimal = animaisList.isNotEmpty ? animaisList[0] : null;
              });
              Future.delayed(const Duration(milliseconds: 200), _nextStep);
            },
            leading: Container(
              width: 48,
              height: 48,
              decoration: BoxDecoration(
                color: Colors.white,
                border: Border.all(color: Colors.grey[200]!),
                borderRadius: BorderRadius.circular(12),
              ),
              child: Icon(Icons.inventory_2_outlined, color: isSelected ? const Color(0xFF00A63E) : Colors.grey[400]),
            ),
            title: l['label'],
            subtitle: l['detail'],
            trailingText: 'Dia ${l['daysActive']}',
          );
        }),
    ];
  }

  List<Widget> _buildAnimalList(FarmContextViewModel farmContext) {
    final animaisList = _selectedLote != null ? (farmContext.animais[_selectedLote!['id']] ?? []) : [];

    return [
      GestureDetector(
        onTap: _prevStep,
        child: const Row(
          children: [
            Icon(Icons.chevron_left, size: 18, color: Color(0xFF00A63E)),
            Text(
              'Voltar para Lote',
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: Color(0xFF00A63E),
              ),
            ),
          ],
        ),
      ),
      const SizedBox(height: 12),
      Row(
        children: [
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
            decoration: BoxDecoration(
              color: Color(_selectedGalpao?['bg'] as int),
              borderRadius: BorderRadius.circular(10),
            ),
            child: Row(
              children: [
                Text(_selectedGalpao?['emoji'] ?? '', style: const TextStyle(fontSize: 14)),
                const SizedBox(width: 4),
                Text(
                  _selectedGalpao?['label'] ?? '',
                  style: TextStyle(fontSize: 11, fontWeight: FontWeight.bold, color: Color(_selectedGalpao?['color'] as int)),
                ),
              ],
            ),
          ),
          const Padding(
            padding: EdgeInsets.symmetric(horizontal: 4),
            child: Icon(Icons.chevron_right, size: 14, color: Colors.grey),
          ),
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
            decoration: BoxDecoration(
              color: Colors.grey[100],
              borderRadius: BorderRadius.circular(10),
            ),
            child: Row(
              children: [
                const Icon(Icons.inventory_2_outlined, size: 12, color: Colors.grey),
                const SizedBox(width: 4),
                Text(
                  _selectedLote?['label'] ?? '',
                  style: const TextStyle(fontSize: 11, fontWeight: FontWeight.bold, color: Color(0xFF616161)),
                ),
              ],
            ),
          ),
        ],
      ),
      const SizedBox(height: 16),
      const Text(
        'QUAL ANIMAL OU GRUPO?',
        style: TextStyle(
          fontSize: 10,
          fontWeight: FontWeight.w900,
          color: Colors.grey,
          letterSpacing: 1.2,
        ),
      ),
      const SizedBox(height: 12),
      if (animaisList.isEmpty)
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 24),
          child: Center(
            child: Text(
              'Nenhum animal ou grupo disponível neste lote.',
              style: TextStyle(color: Colors.grey[600], fontSize: 13),
            ),
          ),
        )
      else
        ...animaisList.map((a) {
          bool isSelected = _selectedAnimal?['id'] == a['id'];
          return _buildSelectCard(
            isSelected: isSelected,
            onTap: () {
              setState(() => _selectedAnimal = a);
            },
            leading: Container(
              width: 48,
              height: 48,
              decoration: BoxDecoration(
                color: isSelected ? const Color(0xFFE8F5E9) : Colors.white,
                border: Border.all(color: isSelected ? const Color(0xFF00A63E) : Colors.grey[200]!),
                borderRadius: BorderRadius.circular(12),
              ),
              child: Center(child: Text(a['tag'], style: const TextStyle(fontSize: 24))),
            ),
            title: a['label'],
            subtitle: a['detail'],
          );
        }),
    ];
  }

  Widget _buildSelectCard({
    required bool isSelected,
    required VoidCallback onTap,
    required Widget leading,
    required String title,
    required String subtitle,
    String? trailingText,
  }) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 10),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(20),
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 200),
          padding: const EdgeInsets.all(12),
          decoration: BoxDecoration(
            color: isSelected ? const Color(0xFFE8F5E9).withValues(alpha: 0.5) : const Color(0xFFF9F9F9),
            borderRadius: BorderRadius.circular(20),
            border: Border.all(
              color: isSelected ? const Color(0xFF00A63E) : const Color(0xFFEEEEEE),
              width: 2,
            ),
          ),
          child: Row(
            children: [
              leading,
              const SizedBox(width: 16),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: TextStyle(
                        fontSize: 15,
                        fontWeight: FontWeight.w900,
                        color: isSelected ? const Color(0xFF1B5E20) : const Color(0xFF1A1A1A),
                      ),
                    ),
                    Text(
                      subtitle,
                      style: TextStyle(
                        fontSize: 12,
                        color: isSelected ? const Color(0xFF2E7D32) : Colors.grey[600],
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                    if (trailingText != null)
                      Padding(
                        padding: const EdgeInsets.only(top: 4),
                        child: Text(
                          trailingText,
                          style: TextStyle(fontSize: 11, color: Colors.grey[400], fontWeight: FontWeight.bold),
                        ),
                      ),
                  ],
                ),
              ),
              if (isSelected)
                Container(
                  width: 28,
                  height: 28,
                  decoration: const BoxDecoration(color: Color(0xFF00A63E), shape: BoxShape.circle),
                  child: const Icon(Icons.check, color: Colors.white, size: 16),
                )
              else
                const Icon(Icons.chevron_right, color: Color(0xFFD1D1D1)),
            ],
          ),
        ),
      ),
    );
  }
}
