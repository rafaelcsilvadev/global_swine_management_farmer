import 'package:flutter/material.dart';

class TaskCard extends StatelessWidget {
  final String title;
  final String subtitle;
  final IconData icon;
  final Color iconColor;
  final Color iconBackgroundColor;
  final String status;
  final Color statusColor;
  final Color statusBackgroundColor;
  final IconData statusIcon;
  
  final String dateText;
  final Color dateColor;
  final Color dateBackgroundColor;
  
  final bool isCompleted;
  final VoidCallback onTap;

  const TaskCard({
    super.key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.iconColor,
    required this.iconBackgroundColor,
    required this.status,
    required this.statusColor,
    required this.statusBackgroundColor,
    this.statusIcon = Icons.schedule,
    required this.dateText,
    this.dateColor = const Color(0xFF475569),
    this.dateBackgroundColor = const Color(0xFFF1F5F9),
    this.isCompleted = false,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(
          color: isCompleted ? const Color(0xFFBBF7D0) : Colors.grey.shade200,
        ),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.02),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(16),
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Row(
              children: [
                // Icon container
                Container(
                  width: 56,
                  height: 56,
                  decoration: BoxDecoration(
                    color: iconBackgroundColor,
                    borderRadius: BorderRadius.circular(16),
                  ),
                  child: Icon(
                    icon,
                    color: iconColor,
                    size: 28,
                  ),
                ),
                const SizedBox(width: 16),
                
                // Text content
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        title,
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.w700,
                          color: isCompleted ? const Color(0xFF94A3B8) : const Color(0xFF0F172A),
                          decoration: isCompleted ? TextDecoration.lineThrough : null,
                        ),
                      ),
                      const SizedBox(height: 2),
                      Text(
                        subtitle,
                        style: TextStyle(
                          fontSize: 13,
                          fontWeight: FontWeight.w500,
                          color: isCompleted ? const Color(0xFF94A3B8) : const Color(0xFF64748B),
                          decoration: isCompleted ? TextDecoration.lineThrough : null,
                        ),
                      ),
                      const SizedBox(height: 8),
                      
                      // Badges
                      Row(
                        children: [
                          // Status Badge
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                            decoration: BoxDecoration(
                              color: statusBackgroundColor,
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Icon(
                                  statusIcon,
                                  size: 14,
                                  color: statusColor,
                                ),
                                const SizedBox(width: 4),
                                Text(
                                  status,
                                  style: TextStyle(
                                    fontSize: 12,
                                    fontWeight: FontWeight.w600,
                                    color: statusColor,
                                  ),
                                ),
                              ],
                            ),
                          ),
                          const SizedBox(width: 8),
                          
                          // Date Badge
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                            decoration: BoxDecoration(
                              color: dateBackgroundColor,
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: Text(
                              dateText,
                              style: TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w600,
                                color: dateColor,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                
                // Chevron
                const Icon(
                  Icons.chevron_right,
                  color: Color(0xFFCBD5E1),
                  size: 24,
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
