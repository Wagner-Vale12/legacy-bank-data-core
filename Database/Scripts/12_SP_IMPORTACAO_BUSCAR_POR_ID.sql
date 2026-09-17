USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_BUSCAR_POR_ID
    @Id INT
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
    WHERE Id = @Id;
END;
GO