import 'package:flutter/material.dart';

class BatchInfoCard extends StatelessWidget {
  final String batchName;
  final String groupName;
  final int animalCount;
  final String phase;
  final String shed;
  final int day;

  const BatchInfoCard({
    super.key,
    required this.batchName,
    required this.groupName,
    required this.animalCount,
    required this.phase,
    required this.shed,
    required this.day,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      decoration: BoxDecoration(
        color: const Color(0xFFEFF6FF), // blue-50
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: const Color(0xFFBFDBFE)), // blue-200
      ),
      child: Row(
        children: [
          const Text(
            '🐷',
            style: TextStyle(fontSize: 28),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  '$batchName • $groupName',
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.w700,
                    color: Color(0xFF1E40AF), // blue-800
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  '$animalCount animais • $phase • $shed • Dia $day',
                  style: const TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.w500,
                    color: Color(0xFF3B82F6), // blue-500
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
