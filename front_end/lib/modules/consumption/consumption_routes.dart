import 'package:go_router/go_router.dart';
import 'pages/consumption_page.dart';

class ConsumptionRoutes {
  static const String consumption = '/consumption';

  static List<RouteBase> routes = [
    GoRoute(
      path: consumption,
      builder: (context, state) => const ConsumptionPage(),
    ),
  ];
}
