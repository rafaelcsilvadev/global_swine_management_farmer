import 'package:flutter/material.dart';

class BirthSummaryCard extends StatelessWidget {
  final int vivos;
  final int natimortos;
  final int mumificados;

  const BirthSummaryCard({
    super.key,
    required this.vivos,
    required this.natimortos,
    required this.mumificados,
  });

  @override
  Widget build(BuildContext context) {
    final int total = vivos + natimortos + mumificados;

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
            'RESUMO',
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
              // Vivos
              Expanded(
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 10),
                  decoration: BoxDecoration(
                    color: const Color(0xFFDCFCE7),
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '$vivos',
                        style: const TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.w900,
                          color: Color(0xFF15803D),
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        'Vivos',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF16A34A),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(width: 8),

              // Natimortos
              Expanded(
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 10),
                  decoration: BoxDecoration(
                    color: const Color(0xFFFEE2E2),
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '$natimortos',
                        style: const TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.w900,
                          color: Color(0xFFB91C1C),
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        'Natimortos',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFFDC2626),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(width: 8),

              // Mumificados
              Expanded(
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 10),
                  decoration: BoxDecoration(
                    color: const Color(0xFFFEE2E2),
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Column(
                    children: [
                      Text(
                        '$mumificados',
                        style: const TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.w900,
                          color: Color(0xFFB91C1C),
                        ),
                      ),
                      const SizedBox(height: 2),
                      const Text(
                        'Mumificados',
                        style: TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFFDC2626),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(height: 12),
          const Divider(color: Color(0xFFF1F5F9), height: 1),
          const SizedBox(height: 12),
          Center(
            child: RichText(
              text: TextSpan(
                style: const TextStyle(
                  fontSize: 13,
                  color: Color(0xFF64748B),
                ),
                children: [
                  const TextSpan(text: 'Total nascidos: '),
                  TextSpan(
                    text: '$total',
                    style: const TextStyle(
                      fontWeight: FontWeight.w900,
                      color: Color(0xFF0F172A),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
