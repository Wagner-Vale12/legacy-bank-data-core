USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_CRIAR
    @NomeArquivo VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.IMPORTACAO
    (
        NomeArquivo,
        Status
    )
    VALUES
    (
        @NomeArquivo,
        'RECEBIDA'
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
