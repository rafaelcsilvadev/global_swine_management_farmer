import 'package:flutter/material.dart';

class DayProgressCard extends StatelessWidget {
  final int completedTasks;
  final int totalTasks;

  const DayProgressCard({
    super.key,
    required this.completedTasks,
    required this.totalTasks,
  });

  @override
  Widget build(BuildContext context) {
    final double progress = totalTasks > 0 ? completedTasks / totalTasks : 0;
    final int remainingTasks = totalTasks - completedTasks;

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 18),
      decoration: BoxDecoration(
        color: Colors.black.withValues(alpha: 0.12),
        borderRadius: BorderRadius.circular(24),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text(
                'Progresso do Dia',
                style: TextStyle(
                  color: Colors.white,
                  fontWeight: FontWeight.w700,
                  fontSize: 15,
                ),
              ),
              Text(
                '$completedTasks/$totalTasks',
                style: const TextStyle(
                  color: Colors.white,
                  fontWeight: FontWeight.w900,
                  fontSize: 16,
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),
          // Progress Bar
          Container(
            height: 10,
            width: double.infinity,
            decoration: BoxDecoration(
              color: Colors.white.withValues(alpha: 0.2),
              borderRadius: BorderRadius.circular(5),
            ),
            child: Stack(
              children: [
                FractionallySizedBox(
                  alignment: Alignment.centerLeft,
                  widthFactor: progress,
                  child: Container(
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(5),
                    ),
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: 10),
          Text(
            '$remainingTasks tarefa${remainingTasks != 1 ? 's' : ''} restante${remainingTasks != 1 ? 's' : ''}',
            style: const TextStyle(
              color: Color(0xFFC8E6C9),
              fontSize: 12,
              fontWeight: FontWeight.w500,
            ),
          ),
        ],
      ),
    );
  }

}
