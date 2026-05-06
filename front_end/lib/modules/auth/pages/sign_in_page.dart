import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import '../widgets/auth_header.dart';
import '../../../shared/widgets/custom_text_field.dart';
import '../../../shared/widgets/custom_button.dart';
import '../../home/home_routes.dart';

class SignInPage extends StatelessWidget {
  const SignInPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          children: [
            const AuthHeader(),
            Padding(
              padding: const EdgeInsets.all(24.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Entrar',
                    style: TextStyle(
                      fontSize: 28,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF1A1A1A),
                    ),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Use sua matrícula e senha fornecidas pelo gerente.',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey,
                    ),
                  ),
                  const SizedBox(height: 32),
                  const CustomTextField(
                    label: 'Matrícula',
                    hintText: 'Digite sua matrícula',
                    prefixIcon: Icons.person_outline,
                  ),
                  const SizedBox(height: 20),
                  const CustomTextField(
                    label: 'Senha',
                    hintText: 'Digite sua senha',
                    prefixIcon: Icons.lock_outline,
                    isPassword: true,
                  ),
                  const SizedBox(height: 40),
                  CustomButton(
                    text: 'Acessar Sistema',
                    onPressed: () {
                      context.go(HomeRoutes.home);
                    },
                  ),
                  const SizedBox(height: 24),
                  const Center(
                    child: Text(
                      'Esqueceu a senha? Fale com o gerente da granja.',
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.grey,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
