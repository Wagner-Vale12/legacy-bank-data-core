USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_CRIAR
    @NomeArquivo VARCHAR(255),
    @HashArquivo CHAR(64)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.IMPORTACAO
    (
        NomeArquivo,
        Status,
        HashArquivo
    )
    VALUES
    (
        @NomeArquivo,
        'RECEBIDA',
        @HashArquivo
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
