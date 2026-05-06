import 'package:go_router/go_router.dart';
import 'pages/home_page.dart';

class HomeRoutes {
  static const String home = '/home';

  static List<RouteBase> routes = [
    GoRoute(
      path: home,
      builder: (context, state) => const HomePage(),
    ),
  ];
}
