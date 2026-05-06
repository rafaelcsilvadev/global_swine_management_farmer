import 'package:flutter/material.dart';

class AuthHeader extends StatelessWidget {
  const AuthHeader({super.key});

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;
    
    return Container(
      width: double.infinity,
      height: size.height * 0.35,
      decoration: const BoxDecoration(
        color: Color(0xFF00A63E),
        borderRadius: BorderRadius.only(
          bottomLeft: Radius.circular(40),
          bottomRight: Radius.circular(40),
        ),
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          const Text(
            '🐷',
            style: TextStyle(fontSize: 80),
          ),
          const SizedBox(height: 16),
          const Text(
            'GLOBAL SWINE',
            style: TextStyle(
              color: Colors.white,
              fontSize: 24,
              fontWeight: FontWeight.bold,
              letterSpacing: 2,
            ),
          ),
          const Text(
            'FARMER',
            style: TextStyle(
              color: Colors.white70,
              fontSize: 16,
              fontWeight: FontWeight.w300,
              letterSpacing: 4,
            ),
          ),
        ],
      ),
    );
  }
}
