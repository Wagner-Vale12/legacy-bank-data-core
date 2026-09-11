USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_MOVIMENTO_LISTAR
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdExterno,
        Conta,
        Tipo,
        Valor,
        DataMovimento,
        CriadoEm
    FROM dbo.MOVIMENTO
    ORDER BY Id;
END;
GO
