import 'package:flutter/material.dart';
import 'dart:math' as math;

class SyncStatusBar extends StatelessWidget {
  final bool isOnline;
  final int pendingItems;
  final bool isSyncing;

  const SyncStatusBar({
    super.key,
    this.isOnline = true,
    this.pendingItems = 0,
    this.isSyncing = false,
  });

  @override
  Widget build(BuildContext context) {
    Color backgroundColor;
    IconData icon;
    String text;
    Widget? trailing;

    if (isSyncing) {
      backgroundColor = Colors.blue.shade600;
      icon = Icons.sync;
      text = 'Sincronizando dados... aguarde';
    } else if (!isOnline) {
      backgroundColor = Colors.orange.shade700;
      icon = Icons.wifi_off;
      text =
          'Sem conexão — $pendingItems apontamento${pendingItems != 1 ? 's' : ''} pendente${pendingItems != 1 ? 's' : ''}';
      trailing = Container(
        padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
        decoration: BoxDecoration(
          color: Colors.white.withValues(alpha: 0.2),
          borderRadius: BorderRadius.circular(4),
        ),
        child: const Text(
          'Toque para sincronizar',
          style: TextStyle(color: Colors.white, fontSize: 10),
        ),
      );
    } else {
      backgroundColor = Color(0xFF00A63E);
      icon = Icons.check_circle_outline;
      text = 'Conectado — Dados sincronizados';
      trailing = const Icon(Icons.wifi, color: Colors.white, size: 14);
    }

    return Container(
      width: double.infinity,
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
      color: backgroundColor,
      child: Row(
        children: [
          if (isSyncing)
            const _RotatingSyncIcon()
          else
            Icon(icon, color: Colors.white, size: 16),
          const SizedBox(width: 10),
          Expanded(
            child: Text(
              text,
              style: const TextStyle(
                color: Colors.white,
                fontSize: 13,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
          if (trailing != null) trailing,
        ],
      ),
    );
  }
}

class _RotatingSyncIcon extends StatefulWidget {
  const _RotatingSyncIcon();

  @override
  State<_RotatingSyncIcon> createState() => _RotatingSyncIconState();
}

class _RotatingSyncIconState extends State<_RotatingSyncIcon>
    with SingleTickerProviderStateMixin {
  late AnimationController _controller;

  @override
  void initState() {
    super.initState();
    _controller = AnimationController(
      duration: const Duration(seconds: 2),
      vsync: this,
    )..repeat();
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AnimatedBuilder(
      animation: _controller,
      builder: (_, child) {
        return Transform.rotate(
          angle: _controller.value * 2 * math.pi,
          child: child,
        );
      },
      child: const Icon(Icons.sync, color: Colors.white, size: 16),
    );
  }
}
