USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_REPROCESSAR
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.IMPORTACAO
    SET
        Status = 'PROCESSANDO',
        TotalRegistros = 0,
        DataProcessamento = NULL,
        MensagemErro = NULL
    WHERE
        Id = @Id
        AND Status = 'ERRO';

    SELECT @@ROWCOUNT;
END;
GO