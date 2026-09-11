USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_MOVIMENTO_BUSCAR_POR_ID
    @Id INT
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
    WHERE Id = @Id;
END;
GO
