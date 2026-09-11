USE LegacyBankDataCore;
GO

IF OBJECT_ID('dbo.MOVIMENTO', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MOVIMENTO
    (
        Id INT IDENTITY(1,1) NOT NULL,
        IdExterno VARCHAR(50) NOT NULL,
        Conta VARCHAR(30) NOT NULL,
        Tipo VARCHAR(10) NOT NULL,
        Valor DECIMAL(18,2) NOT NULL,
        DataMovimento DATE NOT NULL,

        CriadoEm DATETIME2 NOT NULL
            CONSTRAINT DF_MOVIMENTO_CriadoEm
            DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_MOVIMENTO
            PRIMARY KEY (Id),

        CONSTRAINT UQ_MOVIMENTO_IdExterno
            UNIQUE (IdExterno),

        CONSTRAINT CK_MOVIMENTO_Tipo
            CHECK (Tipo IN ('ENTRADA', 'SAIDA')),

        CONSTRAINT CK_MOVIMENTO_Valor
            CHECK (Valor > 0)
    );
END;
GO
