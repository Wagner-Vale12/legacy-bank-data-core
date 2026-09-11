USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_MOVIMENTO_INSERIR
    @IdExterno VARCHAR(50),
    @Conta VARCHAR(30),
    @Tipo VARCHAR(10),
    @Valor DECIMAL(18,2),
    @DataMovimento DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MOVIMENTO
    (
        IdExterno,
        Conta,
        Tipo,
        Valor,
        DataMovimento
    )
    VALUES
    (
        @IdExterno,
        @Conta,
        @Tipo,
        @Valor,
        @DataMovimento
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
