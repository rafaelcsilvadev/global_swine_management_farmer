import 'package:flutter/material.dart';

class ConsumptionStepperWidget extends StatelessWidget {
  final int value;
  final ValueChanged<int> onChange;
  final int min;
  final int max;
  final String label;
  final String? sublabel;
  final String icon;
  final String variant; // 'feed' | 'water'

  const ConsumptionStepperWidget({
    super.key,
    required this.value,
    required this.onChange,
    this.min = 0,
    this.max = 999,
    required this.label,
    this.sublabel,
    required this.icon,
    required this.variant,
  });

  @override
  Widget build(BuildContext context) {
    final bool isFeed = variant == 'feed';

    // Outer container decoration
    final decoration = BoxDecoration(
      color: isFeed ? const Color(0xFFFFFDF5) : const Color(0xFFF0F9FF),
      borderRadius: BorderRadius.circular(20),
      border: Border.all(
        color: isFeed ? const Color(0xFFFEF3C7) : const Color(0xFFE0F2FE),
        width: 2,
      ),
    );

    // Minus button colors
    final minusBgColor = isFeed ? const Color(0xFFFFF7ED) : const Color(0xFFE0F2FE);
    final minusIconColor = isFeed ? const Color(0xFFD97706) : const Color(0xFF0284C7);

    // Plus button colors
    final plusBgColor = isFeed ? const Color(0xFFF59E0B) : const Color(0xFF0284C7);
    final plusIconColor = Colors.white;

    // Value number colors
    final valueColor = isFeed ? const Color(0xFFB45309) : const Color(0xFF0369A1);

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: decoration,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Header Row (Icon & Title/Sublabel)
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

          // Stepper Controls
          Row(
            children: [
              // Minus Button
              Expanded(
                child: SizedBox(
                  height: 64,
                  child: ElevatedButton(
                    onPressed: value > min ? () => onChange(value - 1) : null,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: minusBgColor,
                      foregroundColor: minusIconColor,
                      disabledBackgroundColor: minusBgColor.withValues(alpha: 0.3),
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(16),
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

              // Value Label
              Expanded(
                child: Center(
                  child: Text(
                    '$value',
                    style: TextStyle(
                      fontSize: 48,
                      fontWeight: FontWeight.w900,
                      color: valueColor,
                    ),
                  ),
                ),
              ),

              // Plus Button
              Expanded(
                child: SizedBox(
                  height: 64,
                  child: ElevatedButton(
                    onPressed: value < max ? () => onChange(value + 1) : null,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: plusBgColor,
                      foregroundColor: plusIconColor,
                      disabledBackgroundColor: plusBgColor.withValues(alpha: 0.3),
                      elevation: 0,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(16),
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
