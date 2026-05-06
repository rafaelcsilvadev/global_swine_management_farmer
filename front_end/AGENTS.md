# AGENTS_FLUTTER.md — Global Swine Management Farmer · Módulo Tratador (Frontend)

> **Nota do Arquiteto / Dev Senior:**
> Este documento define as diretrizes arquiteturais inegociáveis para o aplicativo Flutter offline-first. Integração com API .NET 10.
> **Exemplos de telas estão no diretório `/documents`.**

## 1. Stack Tecnológica
- **Core:** Flutter 3.x+, Dart 3.x+
- **Estado:** ChangeNotifier (Provider)
- **HTTP:** Dio
- **Rotas:** GoRouter
- **Persistência:** sqflite (dados locais), flutter_secure_storage (JWT), shared_preferences
- **Arquitetura:** MVVM Modular + **Offline-First**
- **Erros:** Either Pattern (dartz/fpdart)
- **Regra de Ouro:** **UMA CLASSE POR ARQUIVO SEMPRE**

## 2. Estrutura de Pastas Essencial
```
lib/
├── core/ (database, network, storage, errors)
├── modules/ (auth, warehouse, batch, birth, sync)
└── main.dart
```

- **Atenção:** NÃO utilize a pasta `/presentation` dentro dos módulos. Os diretórios relacionados a UI (como `pages`, `view_models` e `widgets`) devem ficar diretamente na raiz do seu respectivo módulo (ex: `lib/modules/auth/pages/`).

## 3. Offline-First: Sincronização e Repositórios
- **Duplo Repositório:** Cada módulo possui um `LocalRepository` (SQLite) e um `RemoteRepository` (Dio).
- **Leitura:** SEMPRE lê do banco local (única fonte da verdade).
- **Escrita:** 
  1. Salva localmente com `isSynced: false` e `syncStatus: pending`.
  2. Tenta enviar à API. Em caso de sucesso, atualiza local para `isSynced: true`.
  3. Em caso de erro/offline, envia a requisição à fila do `SyncQueueService`.

## 4. Sync Queue e Conectividade
- **SyncQueueService:** Armazena requisições pendentes no SQLite (`endpoint`, `method`, `payload`). Processa em background.
- **ConnectivityService:** Monitora a rede e aciona `SyncQueueService.processQueue()` automaticamente ao reconectar.

## 5. Modelagem de Dados
Entidades sincronizáveis exigem rastreamento de estado:
- `id` (UUID gerado no app)
- `serverId` (ID da API, opcional)
- `isSynced` (bool)
- `syncStatus` (enum: pending, syncing, synced, error)

## 6. MVVM com ChangeNotifier
- ViewModels expõem o estado para a View, sem importar bibliotecas de UI (`flutter/material.dart`).
- Repositórios retornam `Either<Failure, T>`. O ViewModel trata erros atualizando o estado e chamando `notifyListeners()`.

## 7. Segurança e Network
- **AuthInterceptor:** Anexa o token JWT do `SecureStorageService` a cada requisição via Dio.
- Trata status HTTP `401` com logout automático do usuário e limpeza do storage local.
- Uso sistemático de falhas (`ServerFailure`, `NetworkFailure`, `CacheFailure`) nas camadas de infra.

## 8. Checklist PR
- [ ] Uma classe por arquivo
- [ ] Models com `isSynced` e `syncStatus`
- [ ] Repositories retornam `Either<Failure, Success>`
- [ ] Escrita salva localmente primeiro, e leitura sempre do SQLite
- [ ] ViewModels limpos (sem import de widgets)
- [ ] Dispose de listeners/controllers implementado