import 'package:flutter/material.dart';

class ConsumptionSummaryCard extends StatelessWidget {
  final int racao;
  final int agua;

  const ConsumptionSummaryCard({
    super.key,
    required this.racao,
    required this.agua,
  });

  @override
  Widget build(BuildContext context) {
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
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            'RESUMO DO APONTAMENTO',
            style: TextStyle(
              fontSize: 11,
              fontWeight: FontWeight.w900,
              color: Color(0xFF94A3B8),
              letterSpacing: 1.2,
            ),
          ),
          const SizedBox(height: 12),
          Row(
            children: [
              // Ração Column
              Expanded(
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 8),
                  decoration: BoxDecoration(
                    color: const Color(0xFFFFFBEB), // Amber-50
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '$racao',
                        style: const TextStyle(
                          fontSize: 30,
                          fontWeight: FontWeight.w900,
                          color: Color(0xFFB45309), // Amber-700
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        '🌾 Sacos de Ração',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFFD97706), // Amber-600
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(width: 12),

              // Água Column
              Expanded(
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 8),
                  decoration: BoxDecoration(
                    color: const Color(0xFFF0F9FF), // Sky-50
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '$agua',
                        style: const TextStyle(
                          fontSize: 30,
                          fontWeight: FontWeight.w900,
                          color: Color(0xFF0369A1), // Sky-700
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        '💧 Caixas d\'água',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF0284C7), // Sky-600
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
