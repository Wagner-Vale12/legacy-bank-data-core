# LegacyBankDataCore

Projeto full stack desenvolvido para simular um fluxo de processamento de dados semelhante ao encontrado em aplicações corporativas e bancárias legadas.

O backend utiliza **.NET Framework 4.7.2, ASP.NET MVC 5, Web API 2, ADO.NET, Stored Procedures e SQL Server**. O frontend foi desenvolvido em **Angular 22 com Bootstrap**.

A aplicação processa arquivos XML de movimentações financeiras, valida o conteúdo através de XSD, controla o ciclo de importação, persiste os dados com transações e permite consultar e reprocessar importações pela interface web.

> Todos os dados utilizados neste projeto são simulados.

---

## Tecnologias

### Backend

- C#
- .NET Framework 4.7.2
- ASP.NET MVC 5
- ASP.NET Web API 2
- ADO.NET
- SQL Server
- Stored Procedures
- Newtonsoft.Json

### Frontend

- Angular 22
- TypeScript
- Angular Router
- HttpClient
- Signals
- RxJS
- Bootstrap 5

### Processamento e testes

- XML
- XSD
- SHA-256
- Transactions
- MSTest

---

## Arquitetura

```text
Angular
  ↓
ASP.NET Web API 2
  ↓
Service
  ↓
Repository
  ↓
ADO.NET
  ↓
Stored Procedures
  ↓
SQL Server
```

O processamento dos arquivos XML segue o fluxo:

```text
Arquivo XML
   ↓
SHA-256
   ↓
Registro da importação
   ↓
Validação XSD
   ↓
Parsing XML
   ↓
Validação de negócio
   ↓
Transaction
   ↓
Movimentos + Importação
   ↓
SQL Server
```

---

## Funcionalidades

- processamento de arquivos XML;
- validação de layout com XSD;
- controle de status das importações;
- idempotência através de SHA-256;
- prevenção de processamento duplicado;
- transações com Commit e Rollback;
- reprocessamento de importações com erro;
- consulta de movimentações;
- dashboard com indicadores;
- filtros e paginação server-side;
- pesquisa por arquivo, conta e ID externo;
- interface responsiva com Angular e Bootstrap;
- tratamento global de exceções;
- testes automatizados com MSTest.

Fluxo principal de importação:

```text
RECEBIDA
   ↓
PROCESSANDO
   ↓
CONCLUIDA
```

Em caso de falha:

```text
PROCESSANDO
   ↓
ERRO
```

Importações com erro podem ser reprocessadas.

---

## Endpoints principais

### Movimentos

```http
GET /api/movimentos
GET /api/movimentos/{id}
POST /api/movimentos
GET /api/movimentos/paginado
```

Exemplo:

```http
GET /api/movimentos/paginado?termo=FRONT&tipo=ENTRADA&pagina=1&tamanhoPagina=10
```

### Importações

```http
GET /api/importacoes
GET /api/importacoes/{id}
GET /api/importacoes/paginado
POST /api/importacoes/processar
PUT /api/importacoes/{id}/reprocessar
```

Exemplo:

```http
GET /api/importacoes/paginado?status=ERRO&pagina=1&tamanhoPagina=10
```

---

## Estrutura do projeto

```text
LegacyBankDataCore
│
├── Database
│   └── Scripts
│
├── LegacyBankDataCore.Web
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Repositories
│   ├── Filters
│   ├── App_Data
│   └── Views
│
├── LegacyBankDataCore.Tests
│
├── LegacyBankDataCore.Angular
│   └── src
│       └── app
│           ├── core
│           ├── models
│           └── pages
│               ├── dashboard
│               ├── importacoes
│               └── movimentos
│
└── LegacyBankDataCore.slnx
```

---

# Como executar o projeto

> **Para executar a aplicação corretamente, é necessário iniciar o backend ASP.NET no Visual Studio e o frontend Angular em um terminal separado.**

## Pré-requisitos

Certifique-se de possuir:

- **Visual Studio com suporte a .NET Framework**
- **.NET Framework 4.7.2**
- **SQL Server ou SQL Server Express**
- **SQL Server Management Studio**
- **Node.js**
- **npm**
- **Angular CLI**

---

### 1. Clone o repositório

```bash
git clone https://github.com/Wagner-Vale12/legacy-bank-data-core.git
cd legacy-bank-data-core
```

---

### 2. Configure o banco de dados

Crie o banco:

```sql
CREATE DATABASE LegacyBankDataCore;
```

Depois execute os scripts disponíveis em:

```text
Database/Scripts
```

> **Importante:** execute os scripts seguindo a ordem numérica dos arquivos.

---

### 3. Configure a conexão com o SQL Server

Abra:

```text
LegacyBankDataCore.Web/Web.config
```

Configure a connection string conforme a sua instância do SQL Server.

Exemplo:

```xml
<connectionStrings>
  <add
    name="LegacyBankDataCore"
    connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=LegacyBankDataCore;Integrated Security=True;"
    providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

### 4. Execute o backend

Abra a solução:

```text
LegacyBankDataCore.slnx
```

no Visual Studio.

Defina:

```text
LegacyBankDataCore.Web
```

como projeto de inicialização.

Execute utilizando:

```text
IIS Express
```

Exemplo de endereço local:

```text
https://localhost:44318
```

> **O backend precisa estar em execução antes de utilizar as funcionalidades do frontend que dependem da API.**

---

### 5. Execute o frontend

Abra outro terminal e acesse:

```bash
cd LegacyBankDataCore.Angular
```

Instale as dependências:

```bash
npm install
```

Inicie o Angular utilizando o proxy da API:

```bash
ng serve --proxy-config proxy.conf.json
```

Depois acesse:

```text
http://localhost:4200
```

> **O `proxy.conf.json` encaminha as chamadas `/api` do Angular para a Web API executada pelo IIS Express.**

---

## Testes

O projeto possui testes automatizados com MSTest cobrindo cenários como:

- leitura de XML;
- geração de SHA-256;
- arquivo inexistente;
- XML válido;
- XML fora do contrato XSD;
- XML malformado.

Estado atual:

```text
7 testes aprovados
0 falhas
```

Também foram validados manualmente cenários de:

- Commit e Rollback;
- reprocessamento;
- idempotência;
- paginação server-side;
- filtros;
- integração Angular → Web API → SQL Server.

O build de produção do Angular também foi executado com sucesso.

---

## Status

O projeto está funcional para o escopo atual, com backend, frontend, processamento XML, persistência, filtros e paginação integrados.

Próximas evoluções possíveis:

- ampliar cobertura de testes;
- adicionar testes automatizados no frontend;
- Dependency Injection no backend;
- Dapper;
- configuração por ambiente;
- melhorias de logging e observabilidade;
- publicação em IIS.

---

## Autor

**Wagner Vale**

GitHub: [Wagner-Vale12](https://github.com/Wagner-Vale12)
