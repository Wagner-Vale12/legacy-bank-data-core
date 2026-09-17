USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_LISTAR
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        NomeArquivo,
        Status,
        TotalRegistros,
        DataRecebimento,
        DataProcessamento,
        MensagemErro,
        HashArquivo
    FROM dbo.IMPORTACAO
    ORDER BY Id DESC;
END;
GO