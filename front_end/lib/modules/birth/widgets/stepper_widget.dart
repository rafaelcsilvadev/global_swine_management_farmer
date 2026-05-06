import 'package:flutter/material.dart';

class StepperWidget extends StatelessWidget {
  final int value;
  final ValueChanged<int> onChange;
  final int min;
  final int max;
  final String label;
  final String? sublabel;
  final String icon;
  final String variant; // 'default' | 'alert'

  const StepperWidget({
    super.key,
    required this.value,
    required this.onChange,
    this.min = 0,
    this.max = 999,
    required this.label,
    this.sublabel,
    required this.icon,
    this.variant = 'default',
  });

  @override
  Widget build(BuildContext context) {
    final bool isAlert = variant == 'alert';

    // Container decoration
    final decoration = BoxDecoration(
      color: isAlert ? const Color(0xFFFEF2F2) : Colors.white,
      borderRadius: BorderRadius.circular(20),
      border: Border.all(
        color: isAlert ? const Color(0xFFFCA5A5) : const Color(0xFFE2E8F0),
        width: 2,
      ),
    );

    // Minus button styling
    final minusBgColor = isAlert ? const Color(0xFFFEE2E2) : const Color(0xFFF1F5F9);
    final minusIconColor = isAlert ? const Color(0xFFB91C1C) : const Color(0xFF475569);

    // Plus button styling
    final plusBgColor = isAlert ? const Color(0xFFDC2626) : const Color(0xFF16A34A);
    final plusIconColor = Colors.white;

    // Value text styling
    final valueStyle = TextStyle(
      fontSize: 48,
      fontWeight: FontWeight.w900,
      color: isAlert ? const Color(0xFFB91C1C) : const Color(0xFF0F172A),
    );

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: decoration,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header info
          Row(
            children: [
              Text(
                icon,
                style: const TextStyle(fontSize: 24),
              ),
              const SizedBox(width: 8),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      label,
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF0F172A),
                      ),
                    ),
                    if (sublabel != null)
                      Text(
                        sublabel!,
                        style: const TextStyle(
                          fontSize: 12,
                          color: Color(0xFF64748B),
                        ),
                      ),
                  ],
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),

          // Stepper buttons & value
          Row(
            children: [
              // Minus button
              Expanded(
                child: SizedBox(
                  height: 56,
                  child: ElevatedButton(
                    onPressed: value > min ? () => onChange(value - 1) : null,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: minusBgColor,
                      foregroundColor: minusIconColor,
                      disabledBackgroundColor: minusBgColor.withValues(alpha: 0.3),
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(14),
                      ),
                      padding: EdgeInsets.zero,
                    ),
                    child: Icon(
                      Icons.remove,
                      size: 28,
                      color: value > min ? minusIconColor : minusIconColor.withValues(alpha: 0.3),
                    ),
                  ),
                ),
              ),

              // Value display
              Expanded(
                child: Center(
                  child: Text(
                    '$value',
                    style: valueStyle,
                  ),
                ),
              ),

              // Plus button
              Expanded(
                child: SizedBox(
                  height: 56,
                  child: ElevatedButton(
                    onPressed: value < max ? () => onChange(value + 1) : null,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: plusBgColor,
                      foregroundColor: plusIconColor,
                      disabledBackgroundColor: plusBgColor.withValues(alpha: 0.3),
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(14),
                      ),
                      padding: EdgeInsets.zero,
                    ),
                    child: Icon(
                      Icons.add,
                      size: 28,
                      color: value < max ? plusIconColor : plusIconColor.withValues(alpha: 0.3),
                    ),
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
