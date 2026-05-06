abstract class Failure {
  final String message;
  Failure(this.message);
}

class ServerFailure extends Failure {
  ServerFailure([String message = 'Erro no servidor']) : super(message);
}

class NetworkFailure extends Failure {
  NetworkFailure([String message = 'Sem conexão com a internet']) : super(message);
}

class CacheFailure extends Failure {
  CacheFailure([String message = 'Erro ao acessar cache local']) : super(message);
}

class AuthFailure extends Failure {
  AuthFailure([String message = 'Erro de autenticação']) : super(message);
}
