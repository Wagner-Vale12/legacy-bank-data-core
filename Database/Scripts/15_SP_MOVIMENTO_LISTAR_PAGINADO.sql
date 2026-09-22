USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_MOVIMENTO_LISTAR_PAGINADO
    @Termo VARCHAR(50) = NULL,
    @Tipo VARCHAR(10) = NULL,
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
        IdExterno,
        Conta,
        Tipo,
        Valor,
        DataMovimento,
        CriadoEm
    FROM dbo.MOVIMENTO
    WHERE
        (
            @Termo IS NULL
            OR IdExterno LIKE '%' + @Termo + '%'
            OR Conta LIKE '%' + @Termo + '%'
        )
        AND
        (
            @Tipo IS NULL
            OR Tipo = @Tipo
        )
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @TamanhoPagina ROWS ONLY;

    SELECT
        COUNT(*) AS TotalRegistros
    FROM dbo.MOVIMENTO
    WHERE
        (
            @Termo IS NULL
            OR IdExterno LIKE '%' + @Termo + '%'
            OR Conta LIKE '%' + @Termo + '%'
        )
        AND
        (
            @Tipo IS NULL
            OR Tipo = @Tipo
        );
END;
GO