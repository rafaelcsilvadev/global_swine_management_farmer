import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class AppBottomNavigationBar extends StatelessWidget {
  final int selectedIndex;

  const AppBottomNavigationBar({
    super.key,
    required this.selectedIndex,
  });

  @override
  Widget build(BuildContext context) {
    return NavigationBarTheme(
      data: NavigationBarThemeData(
        labelTextStyle: WidgetStateProperty.resolveWith((states) {
          if (states.contains(WidgetState.selected)) {
            return const TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: Color(0xFF16A34A),
            );
          }
          return const TextStyle(
            fontSize: 12,
            fontWeight: FontWeight.w500,
            color: Color(0xFF94A3B8),
          );
        }),
        iconTheme: WidgetStateProperty.resolveWith((states) {
          if (states.contains(WidgetState.selected)) {
            return const IconThemeData(color: Color(0xFF16A34A));
          }
          return const IconThemeData(color: Color(0xFF94A3B8));
        }),
      ),
      child: NavigationBar(
        selectedIndex: selectedIndex,
        onDestinationSelected: (int index) {
          if (index == selectedIndex) return;

          switch (index) {
            case 0:
              context.go('/home');
              break;
            case 1:
              context.go('/birth');
              break;
            case 2:
              context.go('/consumption');
              break;
            case 3:
              context.go('/health');
              break;
          }
        },
        backgroundColor: Colors.white,
        indicatorColor: const Color(0xFFDCFCE7), // Light green circle
        surfaceTintColor: Colors.transparent,
        destinations: const [
          NavigationDestination(
            icon: Icon(Icons.home_outlined),
            selectedIcon: Icon(Icons.home),
            label: 'Início',
          ),
          NavigationDestination(
            icon: Icon(Icons.child_care),
            label: 'Partos',
          ),
          NavigationDestination(
            icon: Icon(Icons.eco_outlined),
            selectedIcon: Icon(Icons.eco),
            label: 'Consumo',
          ),
          NavigationDestination(
            icon: Icon(Icons.favorite_border),
            selectedIcon: Icon(Icons.favorite),
            label: 'Saúde',
          ),
        ],
      ),
    );
  }
}
