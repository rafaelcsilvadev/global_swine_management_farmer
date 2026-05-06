import 'package:flutter/foundation.dart';

class FarmContextViewModel extends ChangeNotifier {
  // Mock Data (Ported from ContextSelectorDrawer, using int for colors to avoid UI library imports)
  final List<Map<String, dynamic>> galpoes = const [
    {
      'id': 'A',
      'label': 'Galpão A',
      'type': 'Maternidade',
      'emoji': '🍼',
      'color': 0xFFD81B60,
      'bg': 0xFFFCE4EC,
      'count': 48
    },
    {
      'id': 'B',
      'label': 'Galpão B',
      'type': 'Terminação',
      'emoji': '🐷',
      'color': 0xFF795548,
      'bg': 0xFFFFF8E1,
      'count': 842
    },
    {
      'id': 'C',
      'label': 'Galpão C',
      'type': 'Crescimento',
      'emoji': '🌱',
      'color': 0xFF2E7D32,
      'bg': 0xFFE8F5E9,
      'count': 560
    },
    {
      'id': 'D',
      'label': 'Galpão D',
      'type': 'Gestação',
      'emoji': '🤰',
      'color': 0xFF7B1FA2,
      'bg': 0xFFF3E5F5,
      'count': 120
    },
  ];

  final Map<String, List<Map<String, dynamic>>> lotes = const {
    'A': [
      {
        'id': 'MAT-012',
        'label': 'Lote MAT-012',
        'detail': '3 dias de vida · 11 porcas',
        'animals': 11,
        'daysActive': 3
      },
      {
        'id': 'MAT-011',
        'label': 'Lote MAT-011',
        'detail': '8 dias de vida · 14 porcas',
        'animals': 14,
        'daysActive': 8
      },
    ],
    'B': [
      {
        'id': '2024-047',
        'label': 'Lote 2024-047',
        'detail': 'Dia 87 de terminação · 842 animais',
        'animals': 842,
        'daysActive': 87
      },
      {
        'id': '2024-046',
        'label': 'Lote 2024-046',
        'detail': 'Dia 102 de terminação · 790 animais',
        'animals': 790,
        'daysActive': 102
      },
    ],
    'C': [
      {
        'id': 'CRE-001',
        'label': 'Lote CRE-001',
        'detail': 'Dia 45 de crescimento · 560 animais',
        'animals': 560,
        'daysActive': 45
      },
    ],
    'D': [
      {
        'id': 'GES-001',
        'label': 'Lote GES-001',
        'detail': 'Dia 30 de gestação · 120 animais',
        'animals': 120,
        'daysActive': 30
      },
    ],
  };

  final Map<String, List<Map<String, dynamic>>> animais = const {
    'MAT-012': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todas as 11 porcas do lote'
      },
      {
        'id': 'P041',
        'label': 'Porca #041',
        'tag': '🔴',
        'detail': '2ª Leitegada · Em trabalho de parto'
      },
    ],
    'MAT-011': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todas as 14 porcas do lote'
      },
    ],
    '2024-047': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todos os 842 animais do lote'
      },
    ],
    '2024-046': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todos os 790 animais do lote'
      },
    ],
    'CRE-001': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todos os 560 animais do lote'
      },
    ],
    'GES-001': [
      {
        'id': 'grupo',
        'label': 'Grupo Completo',
        'tag': '👥',
        'detail': 'Todos os 120 animais do lote'
      },
    ],
  };

  late Map<String, dynamic> _selectedGalpao;
  late Map<String, dynamic> _selectedLote;
  late Map<String, dynamic> _selectedAnimal;

  FarmContextViewModel() {
    _selectedGalpao = galpoes[1]; // Default to Galpão B
    _selectedLote = lotes['B']![0];
    _selectedAnimal = animais['2024-047']![0];
  }

  Map<String, dynamic> get selectedGalpao => _selectedGalpao;
  Map<String, dynamic> get selectedLote => _selectedLote;
  Map<String, dynamic> get selectedAnimal => _selectedAnimal;

  void selectContext({
    required Map<String, dynamic> galpao,
    required Map<String, dynamic> lote,
    required Map<String, dynamic> animal,
  }) {
    _selectedGalpao = galpao;
    _selectedLote = lote;
    _selectedAnimal = animal;
    notifyListeners();
  }
}
