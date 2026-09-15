USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_INICIAR_PROCESSAMENTO
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.IMPORTACAO
    SET
        Status = 'PROCESSANDO',
        MensagemErro = NULL
    WHERE
        Id = @Id
        AND Status = 'RECEBIDA';

    SELECT @@ROWCOUNT;
END;
GO
