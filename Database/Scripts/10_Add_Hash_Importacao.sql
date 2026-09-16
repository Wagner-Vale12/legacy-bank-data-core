USE LegacyBankDataCore;
GO

IF COL_LENGTH('dbo.IMPORTACAO', 'HashArquivo') IS NULL
BEGIN
    ALTER TABLE dbo.IMPORTACAO
    ADD HashArquivo CHAR(64) NULL;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE
        name = 'UX_IMPORTACAO_HashArquivo'
        AND object_id = OBJECT_ID('dbo.IMPORTACAO')
)
BEGIN
    CREATE UNIQUE INDEX UX_IMPORTACAO_HashArquivo
    ON dbo.IMPORTACAO(HashArquivo)
    WHERE HashArquivo IS NOT NULL;
END;
GO
