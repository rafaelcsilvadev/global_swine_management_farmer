import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:provider/provider.dart';
import 'modules/auth/auth_routes.dart';
import 'modules/home/home_routes.dart';
import 'modules/birth/birth_routes.dart';
import 'modules/consumption/consumption_routes.dart';
import 'modules/health/health_routes.dart';
import 'modules/home/view_models/farm_context_view_model.dart';

import 'package:intl/date_symbol_data_local.dart';

void main() {
  initializeDateFormatting('pt_BR', null).then((_) {
    runApp(
      ChangeNotifierProvider(
        create: (_) => FarmContextViewModel(),
        child: const MyApp(),
      ),
    );
  });
}

final GoRouter _router = GoRouter(
  initialLocation: AuthRoutes.signIn,
  routes: [
    ...AuthRoutes.routes,
    ...HomeRoutes.routes,
    ...BirthRoutes.routes,
    ...ConsumptionRoutes.routes,
    ...HealthRoutes.routes,
  ],
);

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      title: 'Global Swine Farmer',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF00A63E)),
        useMaterial3: true,
      ),
      routerConfig: _router,
    );
  }
}
