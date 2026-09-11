USE LegacyBankDataCore;
GO

IF OBJECT_ID('dbo.IMPORTACAO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.IMPORTACAO
    (
        Id INT IDENTITY(1,1) NOT NULL,

        NomeArquivo VARCHAR(255) NOT NULL,

        Status VARCHAR(20) NOT NULL,

        TotalRegistros INT NOT NULL
            CONSTRAINT DF_IMPORTACAO_TotalRegistros
            DEFAULT 0,

        DataRecebimento DATETIME2 NOT NULL
            CONSTRAINT DF_IMPORTACAO_DataRecebimento
            DEFAULT SYSUTCDATETIME(),

        DataProcessamento DATETIME2 NULL,

        MensagemErro VARCHAR(1000) NULL,

        CONSTRAINT PK_IMPORTACAO
            PRIMARY KEY (Id),

        CONSTRAINT CK_IMPORTACAO_Status
            CHECK
            (
                Status IN
                (
                    'RECEBIDA',
                    'PROCESSANDO',
                    'CONCLUIDA',
                    'ERRO'
                )
            )
    );
END;
GO
