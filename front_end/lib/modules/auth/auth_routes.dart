import 'package:go_router/go_router.dart';
import 'pages/sign_in_page.dart';

class AuthRoutes {
  static const String signIn = '/signIn';

  static List<RouteBase> routes = [
    GoRoute(
      path: signIn,
      builder: (context, state) => const SignInPage(),
    ),
  ];
}
