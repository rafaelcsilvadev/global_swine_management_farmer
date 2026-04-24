# AGENTS.md — Global Swine Management Farmer · Módulo Tratador

> **Nota do Arquiteto / Dev Senior:**
> Este documento define as diretrizes arquiteturais inegociáveis para o desenvolvimento desta API. Siga estes padrões à risca para garantirmos manutenibilidade, segurança e performance no backend do módulo do Tratador.

## 1. Stack Tecnológico e Arquitetura Base
- **Core:** .NET 10 Web API, Entity Framework Core, PostgreSQL.
- **Infraestrutura:** Docker, Nginx (Reverse Proxy).
- **Padrão de Projeto:** `Controller → Repository → Entity`. Cada módulo de negócio deve ser 100% isolado.
- **Regra de Ouro (SOLID):** **UMA CLASSE POR ARQUIVO SEMPRE**. Esta regra se aplica estritamente a Interfaces, Records (DTOs/Responses), Classes e Validators.

## 2. Visão de Negócio (Fluxo do Tratador)
O fluxo operacional mapeia a rotina diária na granja:
1. Autenticação segura via E-mail e Senha (JWT).
2. Seleção de contexto: Galpão (`Warehouse`) ➔ Lote (`Batch`).
3. Registros de manejo: 
   - Partos (vivos, natimortos, mumificados).
   - Consumo de insumos (ração e água).
   - Saúde animal (sintomas, imagens e medicações aplicadas).

## 3. Modelo de Dados e Auditoria Padrão
- **`BaseEntity`:** Todas as entidades de banco de dados DEVEM obrigatoriamente herdar de `BaseEntity`. Ela garante chaves primárias padronizadas (`Guid Id`) e trilha de auditoria: `CreatedAt`, `UpdatedAt` e `DeletedAt` (preparado para Soft Delete).
- Relacionamentos mapeados via EF Core com forte integridade referencial. Exemplo: `SymptomObserved` atua como junção N:N para sintomas e tratamentos.

## 4. Estrutura do Projeto (Domain-Driven Design simplificado)
Os módulos ficam sob a pasta `src/` e não devem misturar responsabilidades:
- `Data/`: Acesso ao banco (`AppDbContext`) e Migrations.
- `Common/`: Componentes transversais (`BaseEntity`, `JwtMiddleware`).
- `src/[NomeDoModulo]/`: Contendo subpastas `Controllers/`, `DTOs/`, `Entities/` e `Repositories/`.
- **Testes:** Projeto isolado `GlobalSwineManagementFarmer.Tests` contendo testes xUnit espelhando a estrutura da API.

## 5. Segurança, Autenticação e Resiliência
- **JWT (JSON Web Token):** Autenticação Stateless. O token armazena `id`, `ruleId` e `role` em suas Claims.
- Endpoints protegidos utilizando a annotation padrão `[Authorize(Roles = "...")]`.
- Middleware customizado injeta o usuário diretamente no `HttpContext.Items`.
- Senhas protegidas utilizando BCrypt.
- **Rate Limiting:** Implementado nativamente no .NET para mitigar ataques de força bruta (ex: limite no endpoint de login).
- CORS configurado estritamente para o Frontend.

## 6. Tráfego de Dados e Validações (DTOs e Responses)
- **Zero Vazamento de Entidades:** É **PROIBIDO** retornar Entidades do EF Core (Models) diretamente pelas Controllers.
- Utilize exclusivamente **Records** para o tráfego de dados (imutabilidade e performance).
- **DTOs de Entrada:** Sufixo `*Dto` (ex: `SignInDto`).
- **Respostas Padrão:** Toda controller deve retornar um record de resposta (`*Response`, ex: `BatchResponse`). Isso evita ciclos infinitos na serialização JSON e esconde dados sensíveis.
- **Validação de Inputs:** Uso mandatório do **FluentValidation** com validação automática (`AddFluentValidationAutoValidation`). Nenhuma requisição inválida deve chegar aos repositórios.
