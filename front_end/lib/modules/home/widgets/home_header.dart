import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'day_progress_card.dart';

class HomeHeader extends StatelessWidget {
  final String userName;
  final int completedTasks;
  final int totalTasks;

  const HomeHeader({
    super.key,
    required this.userName,
    required this.completedTasks,
    required this.totalTasks,
  });

  @override
  Widget build(BuildContext context) {
    // Format date: "Sexta-Feira, 08 De Maio"
    final DateTime now = DateTime.now();
    final String weekday = DateFormat('EEEE', 'pt_BR').format(now);
    final String day = DateFormat('dd', 'pt_BR').format(now);
    final String month = DateFormat('MMMM', 'pt_BR').format(now);

    // Capitalize first letters like in the mockup
    final String formattedDate = "${_capitalize(weekday)}, $day De ${_capitalize(month)}";

    return Container(
      width: double.infinity,
      color: const Color(0xFF00A63E),
      padding: const EdgeInsets.fromLTRB(16, 12, 16, 24),
      child: Column(
        children: [
          Row(
            children: [
              // User Avatar
              Container(
                width: 48,
                height: 48,
                decoration: BoxDecoration(
                  color: Colors.white.withValues(alpha: 0.2),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.person_outline,
                  color: Colors.white,
                  size: 28,
                ),
              ),
              const SizedBox(width: 12),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Bom dia,',
                    style: TextStyle(
                      color: Color(0xFFC8E6C9),
                      fontSize: 13,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  Text(
                    userName,
                    style: const TextStyle(
                      color: Colors.white,
                      fontSize: 18,
                      fontWeight: FontWeight.w900,
                    ),
                  ),
                ],
              ),
              const Spacer(),
              // Date Badge
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
                decoration: BoxDecoration(
                  color: Colors.black.withValues(alpha: 0.08),
                  borderRadius: BorderRadius.circular(20),
                ),
                child: Row(
                  children: [
                    const Icon(
                      Icons.calendar_month_outlined,
                      color: Colors.white,
                      size: 16,
                    ),
                    const SizedBox(width: 6),
                    Text(
                      formattedDate,
                      style: const TextStyle(
                        color: Colors.white,
                        fontSize: 11,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(width: 8),
              // Logout Button
              Container(
                decoration: BoxDecoration(
                  color: Colors.white.withValues(alpha: 0.1),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: IconButton(
                  onPressed: () {},
                  icon: const Icon(
                    Icons.logout,
                    color: Colors.white,
                    size: 20,
                  ),
                  visualDensity: VisualDensity.compact,
                  padding: EdgeInsets.zero,
                  constraints: const BoxConstraints(
                    minWidth: 40,
                    minHeight: 40,
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(height: 24),
          DayProgressCard(
            completedTasks: completedTasks,
            totalTasks: totalTasks,
          ),
        ],
      ),
    );
  }

  String _capitalize(String text) {
    if (text.isEmpty) return text;
    return text[0].toUpperCase() + text.substring(1);
  }

}
