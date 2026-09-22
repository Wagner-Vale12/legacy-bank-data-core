# LegacyBankDataCore

Projeto focado em **.NET Framework, ASP.NET MVC 5, Web API 2, SQL Server, processamento de arquivos XML e Angular**, simulando um fluxo de dados semelhante ao encontrado em aplicações corporativas e bancárias legadas.

A aplicação recebe arquivos XML contendo movimentações financeiras, valida o layout e as regras de negócio, controla o processamento da importação e persiste os dados no SQL Server utilizando **ADO.NET e Stored Procedures**.

O frontend em **Angular 22 + Bootstrap** consome a Web API para acompanhar importações, visualizar erros, reprocessar arquivos, consultar movimentações e exibir indicadores em um dashboard.

> Todos os dados utilizados no projeto são simulados.

---

## Tecnologias

### Backend

- C#
- .NET Framework 4.7.2
- ASP.NET MVC 5
- ASP.NET Web API 2
- ADO.NET
- System.Web
- Razor
- Newtonsoft.Json

### Frontend

- Angular 22
- TypeScript
- Angular Router
- HttpClient
- Signals
- Computed Signals
- RxJS
- Bootstrap 5

### Banco de dados

- SQL Server
- SQL Server Express
- Stored Procedures
- Transactions
- Commit / Rollback

### Processamento de arquivos

- XML
- XDocument
- XmlReader
- XSD
- SHA-256

### Testes e ambiente

- MSTest
- Visual Studio
- Visual Studio Code
- IIS Express
- Node.js
- npm
- NuGet

---

## Arquitetura

O backend utiliza uma separação entre Controller, Service e Repository.

```text
Angular
   ↓
ASP.NET Web API 2
   ↓
Controller
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

### Backend

- cadastro e consulta de movimentações;
- processamento de arquivos XML;
- validação de XML através de XSD;
- detecção de XML malformado;
- validações de negócio;
- controle do status das importações;
- idempotência através de SHA-256;
- prevenção de arquivos duplicados;
- transações com Commit e Rollback;
- reprocessamento controlado de importações com erro;
- tratamento global de exceções;
- endpoints REST;
- testes automatizados.

Fluxo normal da importação:

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

Importações com erro podem ser reprocessadas:

```text
ERRO
 ↓
PROCESSANDO
 ↓
CONCLUIDA
```

### Frontend

O frontend atualmente possui três áreas principais:

**Dashboard**

- total de importações;
- importações concluídas;
- importações com erro;
- importações em processamento;
- total de movimentações;
- quantidade e valor de entradas;
- quantidade e valor de saídas;
- últimas importações processadas.

**Importações**

- listagem das importações;
- status visual através de badges;
- visualização dos detalhes;
- mensagens de erro do processamento;
- reprocessamento de importações com status `ERRO`;
- confirmação antes do reprocessamento;
- feedback de sucesso ou falha;
- atualização automática da listagem.

**Movimentações**

- consulta das movimentações processadas;
- identificação de `ENTRADA` e `SAIDA`;
- valores formatados em Real brasileiro;
- pesquisa por ID externo;
- pesquisa por conta;
- filtro por tipo;
- contador de resultados;
- limpeza dos filtros.

---

## Principais endpoints

### Movimentos

```http
GET /api/movimentos
GET /api/movimentos/{id}
POST /api/movimentos
```

### Importações

```http
GET /api/importacoes
GET /api/importacoes/{id}

POST /api/importacoes/processar

PUT /api/importacoes/{id}/reprocessar
```

Exemplo de processamento:

```json
{
  "NomeArquivo": "movimentos_exemplo.xml"
}
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
│   ├── App_Data
│   ├── App_Start
│   ├── Controllers
│   ├── Exceptions
│   ├── Filters
│   ├── Models
│   ├── Repositories
│   ├── Services
│   ├── Views
│   ├── Global.asax
│   └── Web.config
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

## Como executar

### Pré-requisitos

Tenha instalado:

- Visual Studio com suporte a ASP.NET / .NET Framework;
- .NET Framework 4.7.2;
- SQL Server ou SQL Server Express;
- SQL Server Management Studio;
- Node.js;
- npm;
- Angular CLI.

### 1. Clone o repositório

```bash
git clone https://github.com/Wagner-Vale12/legacy-bank-data-core.git
cd legacy-bank-data-core
```

### 2. Configure o banco

Crie o banco:

```sql
CREATE DATABASE LegacyBankDataCore;
```

Execute os scripts da pasta:

```text
Database/Scripts
```

seguindo a ordem numérica.

### 3. Configure a connection string

No arquivo:

```text
LegacyBankDataCore.Web/Web.config
```

configure sua instância do SQL Server.

Exemplo:

```xml
<connectionStrings>
  <add
    name="LegacyBankDataCore"
    connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=LegacyBankDataCore;Integrated Security=True;"
    providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 4. Execute o backend

Abra:

```text
LegacyBankDataCore.slnx
```

no Visual Studio.

Defina `LegacyBankDataCore.Web` como projeto de inicialização e execute utilizando **IIS Express**.

Exemplo:

```text
https://localhost:44318
```

### 5. Execute o frontend

Em outro terminal:

```bash
cd LegacyBankDataCore.Angular
npm install
ng serve --proxy-config proxy.conf.json
```

Acesse:

```text
http://localhost:4200
```

O proxy de desenvolvimento encaminha as requisições `/api` do Angular para a Web API .NET.

---

## Testes

O projeto possui testes automatizados com MSTest cobrindo cenários importantes do processamento.

Entre os cenários testados estão:

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

Também foram realizados testes manuais de integração envolvendo:

- processamento de XML;
- transações;
- Commit e Rollback;
- falha durante a conclusão;
- reprocessamento;
- idempotência;
- integração Angular → Web API → SQL Server.

---

## Decisões técnicas

### ADO.NET

O acesso ao banco foi implementado inicialmente com ADO.NET para aprofundar conhecimentos sobre:

```text
SqlConnection
SqlCommand
SqlParameter
SqlDataReader
Transaction
```

Dapper poderá ser introduzido posteriormente como evolução da camada de persistência.

### Stored Procedures

As principais operações de banco utilizam Stored Procedures, reproduzindo um cenário comum em aplicações corporativas legadas.

### SHA-256

Cada arquivo recebe um hash SHA-256 utilizado para identificar conteúdos já registrados e auxiliar no controle de idempotência.

### Transactions

A persistência dos movimentos e a conclusão da importação fazem parte da mesma transação.

Em caso de falha, é executado:

```text
ROLLBACK
```

evitando persistência parcial.

### Angular

O frontend utiliza **Signals** para gerenciamento de estado e `computed()` para informações derivadas, como indicadores do dashboard e resultados filtrados.

As chamadas HTTP ficam centralizadas em services utilizando `HttpClient`.

---

## Status

### Backend

✅ Concluído para o escopo atual.

```text
Web API 2
SQL Server
ADO.NET
Stored Procedures
XML
XSD
SHA-256
Idempotência
Transactions
Commit / Rollback
Reprocessamento
Tratamento global de erros
Testes automatizados
```

### Frontend

✅ Funcional para o escopo atual.

```text
Angular 22
Bootstrap
Routing
HttpClient
Signals
Dashboard
Importações
Detalhes e erros
Reprocessamento
Movimentações
Pesquisa e filtros
Formatação BRL
Integração com Web API
```

---

## Próximas evoluções

- refinamento visual e responsividade;
- paginação;
- filtros server-side;
- ampliação da cobertura de testes;
- Dependency Injection no backend;
- Dapper;
- configuração por ambiente;
- melhorias de logging e observabilidade;
- preparação para publicação em IIS.

---

## Autor

**Wagner Vale**

GitHub: [Wagner-Vale12](https://github.com/Wagner-Vale12)
