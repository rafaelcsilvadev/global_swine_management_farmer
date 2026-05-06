import 'package:go_router/go_router.dart';
import 'pages/health_page.dart';
import 'pages/health_success_page.dart';

class HealthRoutes {
  static const String health = '/health';
  static const String healthSuccess = '/health/success';

  static List<RouteBase> routes = [
    GoRoute(
      path: health,
      builder: (context, state) => const HealthPage(),
    ),
    GoRoute(
      path: healthSuccess,
      builder: (context, state) {
        final extra = state.extra as Map<String, dynamic>? ?? {};
        final symptoms = extra['symptoms'] as List<String>? ?? [];
        final photoCaptured = extra['photoCaptured'] as bool? ?? false;
        return HealthSuccessPage(
          symptoms: symptoms,
          photoCaptured: photoCaptured,
        );
      },
    ),
  ];
}
