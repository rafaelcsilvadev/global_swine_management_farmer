import 'dart:async';
import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import '../../home/home_routes.dart';

class HealthSuccessPage extends StatefulWidget {
  final List<String> symptoms;
  final bool photoCaptured;

  const HealthSuccessPage({
    super.key,
    required this.symptoms,
    required this.photoCaptured,
  });

  @override
  State<HealthSuccessPage> createState() => _HealthSuccessPageState();
}

class _HealthSuccessPageState extends State<HealthSuccessPage> {
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    _timer = Timer(const Duration(milliseconds: 1800), () {
      if (mounted) {
        context.go(HomeRoutes.home);
      }
    });
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF0FDF4), // Light green background (green-50)
      body: Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 24.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              // Icon container
              Container(
                width: 96,
                height: 96,
                decoration: const BoxDecoration(
                  color: Color(0xFFDCFCE7), // Lighter green circle
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.check_circle,
                  size: 56,
                  color: Color(0xFF16A34A),
                ),
              ),
              const SizedBox(height: 24),

              // Title
              const Text(
                'Relatório Enviado!',
                style: TextStyle(
                  fontSize: 28,
                  fontWeight: FontWeight.w900,
                  color: Color(0xFF0F172A),
                ),
              ),
              const SizedBox(height: 12),

              // Message
              const Text(
                'O veterinário receberá os dados para análise remota.',
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: 16,
                  color: Color(0xFF475569),
                  fontWeight: FontWeight.w500,
                  height: 1.4,
                ),
              ),
              const SizedBox(height: 24),

              // Redirect indicator
              const Text(
                'Redirecionando...',
                style: TextStyle(
                  fontSize: 13,
                  color: Color(0xFF94A3B8),
                  fontWeight: FontWeight.w500,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
