import 'package:dio/dio.dart';

class ApiClient {
  final Dio dio;

  ApiClient(this.dio) {
    dio.options.baseUrl = 'https://api.globalswine.com'; // Placeholder
    dio.options.connectTimeout = const Duration(seconds: 5);
    dio.options.receiveTimeout = const Duration(seconds: 3);
    
    dio.interceptors.add(
      LogInterceptor(
        requestBody: true,
        responseBody: true,
      ),
    );
  }
}
