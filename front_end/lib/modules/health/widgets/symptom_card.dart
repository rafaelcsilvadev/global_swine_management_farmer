import 'package:flutter/material.dart';

class SymptomCard extends StatefulWidget {
  final String label;
  final String emoji;
  final bool isSelected;
  final VoidCallback onTap;

  const SymptomCard({
    super.key,
    required this.label,
    required this.emoji,
    required this.isSelected,
    required this.onTap,
  });

  @override
  State<SymptomCard> createState() => _SymptomCardState();
}

class _SymptomCardState extends State<SymptomCard> with SingleTickerProviderStateMixin {
  late AnimationController _controller;
  late Animation<double> _scaleAnimation;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 100),
    );
    _scaleAnimation = Tween<double>(begin: 1.0, end: 0.95).animate(
      CurvedAnimation(parent: _controller, curve: Curves.easeIn),
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  void _handleTapDown(TapDownDetails details) {
    _controller.forward();
  }

  void _handleTapUp(TapUpDetails details) {
    _controller.reverse();
    widget.onTap();
  }

  void _handleTapCancel() {
    _controller.reverse();
  }

  @override
  Widget build(BuildContext context) {
    // Red-50 (0xFFFEF2F2) vs White
    final Color bgColor = widget.isSelected ? const Color(0xFFFEF2F2) : Colors.white;
    // Red-400 (0xFFF87171) vs Slate-200 (0xFFE2E8F0)
    final Color borderColor = widget.isSelected ? const Color(0xFFF87171) : const Color(0xFFE2E8F0);
    // Red-700 (0xFFB91C1C) vs Slate-600 (0xFF475569)
    final Color textColor = widget.isSelected ? const Color(0xFFB91C1C) : const Color(0xFF475569);

    return ScaleTransition(
      scale: _scaleAnimation,
      child: GestureDetector(
        onTapDown: _handleTapDown,
        onTapUp: _handleTapUp,
        onTapCancel: _handleTapCancel,
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 150),
          curve: Curves.easeInOut,
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 14),
          decoration: BoxDecoration(
            color: bgColor,
            borderRadius: BorderRadius.circular(20),
            border: Border.all(
              color: borderColor,
              width: 2,
            ),
            boxShadow: widget.isSelected
                ? [
                    BoxShadow(
                      color: const Color(0xFFFEF2F2).withValues(alpha: 0.4),
                      blurRadius: 4,
                      offset: const Offset(0, 2),
                    )
                  ]
                : null,
          ),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Text(
                widget.emoji,
                style: const TextStyle(fontSize: 28),
              ),
              const SizedBox(height: 8),
              Text(
                widget.label,
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  height: 1.2,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
