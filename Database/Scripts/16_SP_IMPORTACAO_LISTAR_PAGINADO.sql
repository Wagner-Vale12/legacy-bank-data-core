USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_LISTAR_PAGINADO
    @Termo VARCHAR(255) = NULL,
    @Status VARCHAR(20) = NULL,
    @Pagina INT = 1,
    @TamanhoPagina INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT;

    SET @Offset =
        (@Pagina - 1) * @TamanhoPagina;

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
    WHERE
        (
            @Termo IS NULL
            OR NomeArquivo LIKE '%' + @Termo + '%'
        )
        AND
        (
            @Status IS NULL
            OR Status = @Status
        )
    ORDER BY Id DESC
    OFFSET @Offset ROWS
    FETCH NEXT @TamanhoPagina ROWS ONLY;

    SELECT
        COUNT(*) AS TotalRegistros
    FROM dbo.IMPORTACAO
    WHERE
        (
            @Termo IS NULL
            OR NomeArquivo LIKE '%' + @Termo + '%'
        )
        AND
        (
            @Status IS NULL
            OR Status = @Status
        );
END;
GO