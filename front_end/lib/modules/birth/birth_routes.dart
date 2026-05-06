import 'package:go_router/go_router.dart';
import 'pages/birth_page.dart';
import 'pages/birth_success_page.dart';

class BirthRoutes {
  static const String birth = '/birth';
  static const String birthSuccess = '/birth/success/:total';

  static List<RouteBase> routes = [
    GoRoute(
      path: birth,
      builder: (context, state) => const BirthPage(),
    ),
    GoRoute(
      path: birthSuccess,
      builder: (context, state) {
        final totalStr = state.pathParameters['total'] ?? '0';
        final total = int.tryParse(totalStr) ?? 0;
        return BirthSuccessPage(total: total);
      },
    ),
  ];
}
