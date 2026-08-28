# LegacyBankDataCore

Aplicação em desenvolvimento para prática e aperfeiçoamento técnico em **.NET Framework, ASP.NET MVC 5, ASP.NET Web API 2, Angular, SQL Server e processamento de XML**, simulando um cenário corporativo próximo ao encontrado em sistemas bancários legados.

O projeto representa um pequeno **Data Core financeiro**, responsável por receber arquivos XML contendo movimentações, validar e processar essas informações, aplicar regras de negócio, persistir os dados no SQL Server e disponibilizá-los para consumo através de APIs e de uma interface web em Angular.

> Projeto em desenvolvimento e evolução contínua.

---

## Objetivo

O objetivo do `LegacyBankDataCore` é reproduzir uma arquitetura híbrida comum em ambientes corporativos que possuem um backend legado em **.NET Framework** e um frontend mais moderno.

A arquitetura planejada é:

```text
Arquivo XML
    ↓
Ingestão
    ↓
Validação
    ↓
Parsing
    ↓
Objetos C#
    ↓
Regras de negócio
    ↓
Service
    ↓
Repository
    ↓
ADO.NET / Dapper
    ↓
Stored Procedures
    ↓
SQL Server
    ↓
ASP.NET Web API 2
    ↓
HTTP / JSON
    ↓
Angular
    ↓
Bootstrap
    ↓
Usuário
```

O projeto também utiliza **ASP.NET MVC 5 + Razor** em partes específicas para aprofundar conhecimentos sobre aplicações legadas e compreender estruturas que ainda podem coexistir com Web API e Angular em sistemas corporativos antigos.

---

# Stack

## Backend

- C#
- .NET Framework 4.7.2
- ASP.NET MVC 5
- ASP.NET Web API 2
- System.Web
- Razor
- Newtonsoft.Json
- NuGet
- packages.config

> O projeto utiliza `.NET Framework 4.7.2` localmente para reproduzir a arquitetura clássica do .NET Framework com tooling moderno, mantendo conceitos compatíveis com sistemas baseados em versões anteriores, como .NET Framework 4.5.

---

## Frontend

Planejado como interface principal da aplicação:

- Angular
- TypeScript
- Angular HttpClient
- Reactive Forms
- Bootstrap
- HTML5
- CSS
- JavaScript

A aplicação Angular será responsável pela interface principal utilizada pelo usuário.

Fluxo:

```text
Angular
   ↓
HttpClient
   ↓
ASP.NET Web API 2
   ↓
Service
   ↓
Repository
   ↓
SQL Server
```

O Bootstrap será utilizado para acelerar a construção de uma interface corporativa, responsiva e funcional.

---

## Banco de dados

Planejado:

- SQL Server
- ADO.NET
- `SqlConnection`
- `SqlCommand`
- `SqlParameter`
- `SqlDataReader`
- Dapper
- Stored Procedures
- Transactions
- Commit / Rollback

---

## Processamento de XML

Planejado:

- `XmlReader`
- `XDocument`
- `XmlDocument`
- `XmlSerializer`
- XSD
- parsing
- validação estrutural
- validação de negócio

---

## Infraestrutura

- Visual Studio
- IIS Express
- IIS
- Application Pool
- Web.config
- Global.asax
- App_Start
- RouteConfig
- WebApiConfig
- NuGet

---

# Arquitetura planejada

```text
                        XML
                         ↓
                ImportacaoController
                         ↓
                  ImportacaoService
                         ↓
          XmlReader / XmlSerializer
                         ↓
               Validação estrutural
                         ↓
                Regras de negócio
                         ↓
                     Service
                         ↓
                    Repository
                         ↓
                ADO.NET / Dapper
                         ↓
               Stored Procedures
                         ↓
                    SQL Server
                         ↓
                  Web API 2
                         ↓
                    HTTP/JSON
                         ↓
                      Angular
                         ↓
                    Bootstrap
                         ↓
                      Usuário
```

---

# Estrutura atual

Atualmente a solução possui:

```text
LegacyBankDataCore
│
└── LegacyBankDataCore.Web
    │
    ├── App_Start
    │   ├── RouteConfig.cs
    │   └── WebApiConfig.cs
    │
    ├── Controllers
    │   ├── HomeController.cs
    │   └── MovimentosController.cs
    │
    ├── Models
    │   └── MovimentoViewModel.cs
    │
    ├── Views
    │   └── Home
    │       ├── Index.cshtml
    │       └── Novo.cshtml
    │
    ├── Global.asax
    ├── Web.config
    ├── packages.config
    └── LegacyBankDataCore.Web.csproj
```

Posteriormente será adicionado o frontend:

```text
LegacyBankDataCore
│
├── LegacyBankDataCore.Web
│
└── LegacyBankDataCore.Angular
```

---

# ASP.NET MVC 5

O projeto possui uma implementação MVC para prática e compreensão da arquitetura clássica do ASP.NET.

Fluxo:

```text
Request
   ↓
RouteConfig
   ↓
Controller
   ↓
Action
   ↓
Model
   ↓
Razor View
   ↓
HTML
```

Atualmente já estão implementados exemplos de:

- Controller MVC
- `ActionResult`
- Razor Views
- ViewModel
- Model Binding
- GET
- POST
- HTML Helpers
- formatação monetária
- formatação de datas

Exemplo:

```text
GET /Home/Novo
       ↓
HomeController
       ↓
View Razor
       ↓
Formulário
       ↓
POST /Home/Novo
       ↓
Model Binding
       ↓
MovimentoViewModel
```

O MVC não será necessariamente o frontend principal da aplicação final, mas continuará presente como parte importante da arquitetura legada e para entendimento de sistemas corporativos que misturam MVC, Web API e frontends modernos.

---

# ASP.NET Web API 2

A aplicação também possui **ASP.NET Web API 2** configurada no mesmo projeto ASP.NET Framework.

A infraestrutura utiliza:

```text
System.Web.Http
ApiController
IHttpActionResult
WebApiConfig
```

Endpoint inicial:

```http
GET /api/movimentos
```

Exemplo de resposta:

```json
{
  "IdExterno": "API001",
  "Conta": "12345",
  "Tipo": "ENTRADA",
  "Valor": 2500
}
```

Fluxo atual:

```text
GET /api/movimentos
        ↓
WebApiConfig
        ↓
MovimentosController
        ↓
ApiController
        ↓
IHttpActionResult
        ↓
JSON
```

A Web API será futuramente responsável pela comunicação principal entre o backend .NET Framework e o frontend Angular.

---

# MVC e Web API no mesmo projeto

Uma característica importante do ecossistema ASP.NET clássico é a coexistência entre MVC e Web API.

## MVC

```text
System.Web.Mvc
      ↓
Controller
      ↓
ActionResult
      ↓
Razor
      ↓
HTML
```

## Web API 2

```text
System.Web.Http
      ↓
ApiController
      ↓
IHttpActionResult
      ↓
JSON / XML
```

Apesar de coexistirem na mesma aplicação ASP.NET Framework, MVC e Web API possuem infraestruturas e mecanismos de roteamento diferentes.

---

# Angular

O frontend principal será desenvolvido em Angular.

A aplicação deverá consumir endpoints da Web API como:

```http
GET /api/movimentos
GET /api/movimentos/{id}

GET /api/importacoes
GET /api/importacoes/{id}

POST /api/importacoes

POST /api/importacoes/{id}/reprocessar
```

Estrutura planejada:

```text
LegacyBankDataCore.Angular
│
├── core
│   └── services
│
├── features
│   ├── dashboard
│   ├── importacoes
│   └── movimentos
│
├── models
│
└── shared
```

A comunicação seguirá:

```text
Angular Component
        ↓
Angular Service
        ↓
HttpClient
        ↓
ASP.NET Web API 2
        ↓
Service C#
        ↓
Repository
        ↓
SQL Server
```

---

# Interface planejada

O frontend deverá possuir inicialmente:

### Dashboard

Resumo do processamento diário.

```text
Importações concluídas
Importações com erro
Quantidade de movimentações
Últimos arquivos recebidos
```

### Importações

Tabela contendo:

```text
Arquivo
Data de recebimento
Quantidade de registros
Status
Data de processamento
```

Estados previstos:

```text
Recebida
Processando
Concluída
Erro
```

### Movimentações

Consulta das movimentações processadas:

```text
Id Externo
Conta
Tipo
Valor
Data
Arquivo de origem
```

### Erros

Visualização dos erros ocorridos durante processamento.

### Reprocessamento

Possibilidade de solicitar novamente o processamento de uma importação com erro.

---

# Bootstrap

O Bootstrap será utilizado junto ao Angular para construir rapidamente componentes visuais como:

- Navbar
- Sidebar
- Cards
- Tabelas
- Formulários
- Modais
- Alerts
- Badges de status
- Paginação
- Grid responsivo

Exemplo conceitual:

```text
┌───────────────────────────────────────────────┐
│ LegacyBankDataCore                            │
├───────────────────────────────────────────────┤
│ Dashboard | Importações | Movimentações       │
├───────────────────────────────────────────────┤
│                                               │
│ Importações recentes                          │
│                                               │
│ Arquivo       Data       Registros   Status   │
│ MOV001.xml    14/09      1250        OK       │
│ MOV002.xml    15/09      820         Erro     │
│                                               │
│ [ Importar XML ]                              │
│                                               │
└───────────────────────────────────────────────┘
```

---

# Inicialização da aplicação

Diferentemente de aplicações ASP.NET Core modernas, o projeto não utiliza `Program.cs`.

A aplicação utiliza o ciclo de vida clássico:

```text
IIS / IIS Express
        ↓
System.Web
        ↓
Global.asax
        ↓
Application_Start()
        ↓
WebApiConfig
        ↓
RouteConfig
```

Atualmente:

```csharp
protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();

    GlobalConfiguration.Configure(WebApiConfig.Register);

    RouteConfig.RegisterRoutes(RouteTable.Routes);
}
```

---

# Configuração

A configuração principal da aplicação utiliza:

```text
Web.config
```

Atualmente estão presentes configurações relacionadas a:

- ASP.NET
- .NET Framework
- runtime
- compilação
- Razor
- CodeDOM
- binding redirects

Posteriormente será adicionada a configuração do SQL Server:

```xml
<connectionStrings>
```

Fluxo esperado:

```text
Web.config
    ↓
connectionStrings
    ↓
ConfigurationManager
    ↓
SqlConnection
    ↓
SQL Server
```

---

# Gerenciamento de dependências

A aplicação utiliza o modelo clássico do NuGet baseado em:

```text
packages.config
```

Principais pacotes atuais:

- Microsoft.AspNet.Mvc
- Microsoft.AspNet.Razor
- Microsoft.AspNet.WebPages
- Microsoft.AspNet.WebApi.Core
- Microsoft.AspNet.WebApi.WebHost
- Microsoft.AspNet.WebApi.Client
- Newtonsoft.Json
- Microsoft.CodeDom.Providers.DotNetCompilerPlatform

---

# Cenário de negócio

O sistema deverá receber diariamente arquivos XML contendo movimentações financeiras.

Exemplo:

```xml
<?xml version="1.0" encoding="utf-8"?>

<movimento>
    <idExterno>ABC123</idExterno>
    <conta>12345</conta>
    <tipo>ENTRADA</tipo>
    <valor>1500.00</valor>
    <data>2026-09-14</data>
</movimento>
```

Fluxo:

```text
XML
 ↓
Importação
 ↓
Validação
 ↓
Parsing
 ↓
Movimento C#
 ↓
Regra de negócio
 ↓
Service
 ↓
Repository
 ↓
Stored Procedure
 ↓
SQL Server
```

---

# Controle de importação

Cada arquivo processado deverá possuir controle de status.

Fluxo normal:

```text
Recebida
   ↓
Processando
   ↓
Concluída
```

Em caso de falha:

```text
Recebida
   ↓
Processando
   ↓
Erro
```

Também serão implementados:

- identificação única do arquivo;
- prevenção de processamento duplicado;
- idempotência;
- histórico de processamento;
- registros de erro;
- auditoria;
- rastreabilidade;
- reprocessamento.

---

# Persistência

A camada de persistência será desenvolvida progressivamente.

Primeiro:

```text
ADO.NET
   ↓
SqlConnection
   ↓
SqlCommand
   ↓
SqlParameter
   ↓
SqlDataReader
```

Depois:

```text
Dapper
   ↓
Repository
   ↓
Stored Procedures
```

O objetivo é compreender primeiro o funcionamento de baixo nível do acesso ao SQL Server antes de utilizar abstrações como Dapper.

---

# Stored Procedures

Stored Procedures terão papel importante na aplicação.

Fluxo previsto:

```text
Controller
   ↓
Service
   ↓
Repository
   ↓
Dapper / ADO.NET
   ↓
Stored Procedure
   ↓
SQL Server
```

Também serão implementados cenários com:

```text
BEGIN TRANSACTION
COMMIT
ROLLBACK
```

para operações que exigem consistência.

---

# Roadmap

## Implementado

- [x] Solution .NET Framework
- [x] .NET Framework 4.7.2
- [x] ASP.NET MVC 5
- [x] Global.asax
- [x] Application_Start
- [x] App_Start
- [x] RouteConfig
- [x] Controller MVC
- [x] ActionResult
- [x] Razor Views
- [x] ViewModel
- [x] Model Binding
- [x] GET MVC
- [x] POST MVC
- [x] HTML Helpers
- [x] Web.config
- [x] packages.config
- [x] NuGet
- [x] ASP.NET Web API 2
- [x] WebApiConfig
- [x] ApiController
- [x] IHttpActionResult
- [x] Endpoint REST inicial
- [x] Serialização JSON

---

## Backend — próximas etapas

- [ ] SQL Server
- [ ] Connection Strings
- [ ] ConfigurationManager
- [ ] ADO.NET
- [ ] SqlConnection
- [ ] SqlCommand
- [ ] SqlParameter
- [ ] SqlDataReader
- [ ] Transactions
- [ ] Dapper
- [ ] Stored Procedures
- [ ] Repository Pattern
- [ ] Service Layer
- [ ] Dependency Injection
- [ ] tratamento global de erros

---

## XML

- [ ] importação de arquivos XML
- [ ] XmlReader
- [ ] XDocument
- [ ] XmlDocument
- [ ] XmlSerializer
- [ ] validação XSD
- [ ] parsing
- [ ] validação estrutural
- [ ] validação de negócio
- [ ] persistência dos registros

---

## Controle de processamento

- [ ] cadastro da importação
- [ ] status Recebida
- [ ] status Processando
- [ ] status Concluída
- [ ] status Erro
- [ ] idempotência
- [ ] prevenção de duplicidade
- [ ] auditoria
- [ ] rastreabilidade
- [ ] logs
- [ ] reprocessamento

---

## Angular

- [ ] criação da aplicação Angular
- [ ] configuração do HttpClient
- [ ] integração com Web API 2
- [ ] configuração de CORS
- [ ] Models TypeScript
- [ ] Services
- [ ] Reactive Forms
- [ ] Bootstrap
- [ ] roteamento Angular
- [ ] tratamento de erros HTTP

---

## Interface

- [ ] layout principal
- [ ] navbar
- [ ] dashboard
- [ ] tela de importações
- [ ] upload de XML
- [ ] lista de movimentações
- [ ] pesquisa
- [ ] filtros
- [ ] detalhes da movimentação
- [ ] detalhes da importação
- [ ] status visual com badges
- [ ] visualização de erros
- [ ] ação de reprocessamento
- [ ] paginação
- [ ] interface responsiva

---

## Infraestrutura e manutenção

- [ ] Debugging de aplicações legadas
- [ ] Breakpoints
- [ ] Watch
- [ ] Call Stack
- [ ] análise de fluxo
- [ ] IIS
- [ ] Application Pool
- [ ] publicação em IIS
- [ ] Web.Debug.config
- [ ] Web.Release.config

---

# Fluxo final esperado

```text
                    Arquivo XML
                         ↓
                  Importação
                         ↓
                   Validação
                         ↓
               Parser / XmlReader
                         ↓
                 Objetos C#
                         ↓
                Regras de negócio
                         ↓
                     Service
                         ↓
                    Repository
                         ↓
                ADO.NET / Dapper
                         ↓
               Stored Procedures
                         ↓
                    SQL Server
                         ↓
                  Web API 2
                         ↓
                      JSON
                         ↓
                     Angular
                         ↓
                    Bootstrap
                         ↓
                      Usuário
```

---

# Próxima etapa

A próxima evolução será iniciar a persistência com:

```text
SQL Server
    ↓
Web.config
    ↓
connectionStrings
    ↓
ConfigurationManager
    ↓
ADO.NET
    ↓
SqlConnection
```

Depois serão adicionados Dapper, Stored Procedures e a camada de Service/Repository.

Com o backend estabilizado, será criada a aplicação Angular responsável por consumir a Web API 2.

---

# Status

🚧 **Em desenvolvimento**

O `LegacyBankDataCore` está sendo evoluído de forma incremental com foco no aperfeiçoamento de práticas utilizadas em aplicações corporativas e bancárias baseadas em **.NET Framework legado, SQL Server, processamento de XML e frontend Angular**.
