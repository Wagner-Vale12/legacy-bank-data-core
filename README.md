# LegacyBankDataCore

Projeto focado em **.NET Framework, ASP.NET MVC 5, Web API 2, SQL Server e processamento de arquivos XML**, simulando um fluxo de dados semelhante ao encontrado em aplicações corporativas e bancárias legadas.

A aplicação recebe arquivos XML contendo movimentações financeiras, valida o layout e as regras de negócio, controla o processamento da importação e persiste os dados no SQL Server utilizando **ADO.NET e Stored Procedures**.

O projeto foi desenvolvido para aprofundar conhecimentos em manutenção e evolução de aplicações .NET legadas, especialmente cenários envolvendo processamento de dados, transações, XML e integração com banco de dados.

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

### Banco de dados

- SQL Server
- SQL Server Express
- Stored Procedures
- Transactions
- Commit / Rollback

### XML

- XDocument
- XmlReader
- XSD
- SHA-256

### Testes

- MSTest

### Ambiente

- Visual Studio
- IIS Express
- NuGet
- packages.config

---

## Arquitetura

O backend utiliza uma separação simples entre Controller, Service e Repository.

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
Repository
     ↓
ADO.NET
     ↓
Stored Procedure
     ↓
SQL Server
```

Para processamento de arquivos XML:

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

O backend atualmente possui:

- cadastro e consulta de movimentações;
- controle de importações;
- processamento de arquivos XML;
- validação de XML através de XSD;
- detecção de XML malformado;
- validações de negócio;
- controle de status da importação;
- idempotência através de SHA-256;
- prevenção de arquivos duplicados;
- transações com Commit e Rollback;
- reprocessamento controlado de importações com erro;
- tratamento global de exceções;
- endpoints REST para consulta das importações;
- testes automatizados.

Estados utilizados durante o processamento:

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
│   │   ├── Importacoes
│   │   └── Schemas
│   │
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
└── LegacyBankDataCore.slnx
```

---

## Como executar

### Pré-requisitos

Tenha instalado:

- Visual Studio com suporte a ASP.NET / .NET Framework;
- .NET Framework 4.7.2;
- SQL Server ou SQL Server Express;
- SQL Server Management Studio.

### 1. Clone o repositório

```bash
git clone https://github.com/Wagner-Vale12/legacy-bank-data-core.git
```

Entre na pasta:

```bash
cd legacy-bank-data-core
```

---

### 2. Configure o banco

Crie o banco:

```sql
CREATE DATABASE LegacyBankDataCore;
```

Depois execute os scripts localizados em:

```text
Database/Scripts
```

seguindo a ordem numérica.

---

### 3. Configure a connection string

No arquivo:

```text
LegacyBankDataCore.Web/Web.config
```

ajuste a connection string conforme sua instalação do SQL Server.

Exemplo utilizando SQL Server Express:

```xml
<connectionStrings>
  <add
    name="LegacyBankDataCore"
    connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=LegacyBankDataCore;Integrated Security=True;"
    providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

### 4. Abra a solução

Abra:

```text
LegacyBankDataCore.slnx
```

no Visual Studio.

Restaure os pacotes NuGet caso necessário.

Depois execute utilizando:

```text
IIS Express
```

---

## Testes

O projeto possui uma suíte MSTest cobrindo partes importantes do processamento.

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

---

## Decisões técnicas

### ADO.NET antes de Dapper

O acesso ao banco foi implementado inicialmente com ADO.NET para aprofundar o entendimento de:

```text
SqlConnection
SqlCommand
SqlParameter
SqlDataReader
Transaction
```

Dapper poderá ser introduzido posteriormente como evolução da camada de persistência.

### Stored Procedures

O acesso aos principais dados utiliza Stored Procedures para reproduzir um cenário comum em sistemas corporativos legados.

### SHA-256 para idempotência

Cada arquivo recebe um hash SHA-256.

Isso permite identificar tentativas de processamento do mesmo conteúdo e evitar importações duplicadas.

### Transação no processamento

A persistência dos movimentos e a conclusão da importação fazem parte da mesma transação.

Em caso de falha:

```text
ROLLBACK
```

é executado para impedir persistência parcial.

### Reprocessamento

Importações com status `ERRO` podem ser reprocessadas utilizando o mesmo registro de importação.

```text
ERRO
 ↓
PROCESSANDO
 ↓
CONCLUIDA
```

O hash original também é utilizado para verificar se o conteúdo do arquivo foi alterado.

---

## Status

### Backend

✅ Backend concluído para o escopo atual do projeto.

Implementado:

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

🚧 Próxima etapa:

```text
Angular
+
Bootstrap
```

O frontend consumirá a Web API já existente para apresentar importações, movimentações, erros e ações de reprocessamento.

---

## Próximas evoluções

- Angular;
- Bootstrap;
- dashboard;
- tela de importações;
- tela de movimentações;
- detalhes de processamento;
- reprocessamento pela interface;
- ampliação da cobertura de testes;
- Dependency Injection;
- Dapper;
- configuração por ambiente.

---

## Autor

**Wagner Vale**

GitHub: [Wagner-Vale12](https://github.com/Wagner-Vale12)
