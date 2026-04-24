# Global Swine Management Farmer

Um sistema de gerenciamento moderno para granjas suínas, focado em fornecer aos tratadores (farmers) e administradores uma ferramenta robusta para o controle diário das operações. O sistema conta com uma API Back-End robusta desenvolvida em `.NET 10`.

## 🚀 Tecnologias Utilizadas

- **.NET 10** (ASP.NET Core Web API)
- **Entity Framework Core**
- **PostgreSQL**
- **Autenticação e Autorização via JWT**
- **FluentValidation** (Validação de DTOs)
- **xUnit & Moq** (Testes Unitários e de Integração)
- **Docker & Nginx**

---

## 🤖 Desenvolvimento Orientado por I.A (Antigravity)

Este projeto foi construído utilizando **Programação Orientada por I.A** em formato de pair programming com o **Antigravity**.

**O que é?**
O Antigravity é um assistente avançado de inteligência artificial focado em engenharia de software e criado pela equipe do Google DeepMind. O desenvolvimento guiado por IA (AI-Driven Programming) é um paradigma moderno onde o desenvolvedor (ou arquiteto de software) atua como um "diretor" do projeto, colaborando ativamente com agentes autônomos para projetar, codificar, refatorar e testar o código-fonte de forma ágil.

Neste projeto, o uso do Antigravity acelerou tarefas como:
- Aplicação rigorosa dos princípios de Clean Code, TDD (Test-Driven Development) e responsabilidade única.
- Desenvolvimento das Entidades, DTOs, Repositórios e Controllers.
- Configuração de bibliotecas de validação (FluentValidation) e de testes automatizados (xUnit, Moq).
- Gestão de configurações e auditorias de segurança no back-end.

---

## 🏛 Arquitetura do Back-End

O projeto adota um padrão de arquitetura focado em separação de responsabilidades (Separation of Concerns), isolado por módulos de negócio (Feature-based structure). 

**Padrão Utilizado:** `Controller` → `Repository` → `Entity`

Cada módulo é estritamente organizado para seguir o princípio de responsabilidade única (uma classe por arquivo) e é composto por:
- **Controllers**: Lidam com as requisições HTTP, rotas e regras de autorização (`Roles = "admin,farmer"`).
- **Repositories**: Contêm as regras de acesso a dados usando Entity Framework, encapsulando as lógicas de negócio.
- **DTOs**: Objetos de Transferência de Dados, validados rigorosamente com `FluentValidation`.
- **Entities**: Mapeamento objeto-relacional (ORM) das tabelas de banco de dados.

### Módulos Principais
1. **Auth**: Autenticação (Login) e registro, retornando JWT tokens baseados em *Roles*.
2. **Warehouse (Galpões)**: Gerenciamento dos galpões da granja e títulos.
3. **Batch (Lotes)**: Gestão dos lotes de animais abrigados nos galpões.
4. **PigBirth (Partos)**: Registro de nascidos vivos, natimortos e mumificados.
5. **Consumption (Consumo)**: Controle de uso de ração e água por lote/galpão.
6. **HealthMedication & Symptom (Saúde)**: Registro de sintomas observados e eficácia de medicações aplicadas.

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker e Docker Compose](https://www.docker.com/)
- Um banco de dados PostgreSQL rodando localmente (ou via Docker).

### 1. Configurando o Banco de Dados e Ambiente

O projeto possui um arquivo `.env` para gerenciar as credenciais no desenvolvimento local (incluindo o banco de dados e segredos do JWT). Existe um arquivo de exemplo preparado para facilitar a configuração inicial.

```bash
cd back_end/GlobalSwineManagementFarmer.Api

# Copie o arquivo de exemplo para gerar o seu .env local
cp .env.example .env
```
*Após copiar, abra o arquivo `.env` e preencha as variáveis com os valores desejados para o seu ambiente local.*

Depois, aplique as Migrations para gerar o banco de dados:

```bash
dotnet ef database update
```

### 2. Rodando a API

```bash
dotnet run
```
A API estará disponível em `http://localhost:5000` ou HTTPS configurado e o Swagger em `/swagger`.

### 3. Rodando os Testes

O projeto possui uma cobertura extensiva de testes TDD.
```bash
cd ../../back_end/GlobalSwineManagementFarmer.Tests
dotnet test
```

### 4. Infraestrutura e Servidor

As configurações de implantação e infraestrutura do servidor (como configurações do Nginx, Docker Compose de produção e afins) encontram-se isoladas no projeto **`global_swine_management_server`**.


---

## 🔐 Autenticação & Regras de Acesso

A aplicação é protegida por um Custom JWT Middleware. Todos os usuários efetuam login via `/api/auth/signin`.
A role do usuário define suas permissões:
- **Admin**: Acesso total de leitura e escrita em todos os endpoints, incluindo registro de novos usuários e manipulação de nomenclaturas.
- **Farmer (Tratador)**: Permissão para realizar registros operacionais diários (Partos, Consumo, Saúde, Lotes) e consultas, mas sem privilégios destrutivos de sistema.

---

## 📝 Documentação Adicional

Desenvolvedores e Agentes de IA trabalhando no projeto devem impreterivelmente consultar o arquivo [`back_end/AGENTS.md`](./back_end/AGENTS.md) para as regras de negócio granulares, padrões de projeto estritos e referências de estruturação das classes.
