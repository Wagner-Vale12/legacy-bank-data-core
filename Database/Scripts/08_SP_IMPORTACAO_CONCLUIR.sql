USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_CONCLUIR
    @Id INT,
    @TotalRegistros INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.IMPORTACAO
    SET
        Status = 'CONCLUIDA',
        TotalRegistros = @TotalRegistros,
        DataProcessamento = SYSUTCDATETIME(),
        MensagemErro = NULL
    WHERE
        Id = @Id
        AND Status = 'PROCESSANDO';

    SELECT @@ROWCOUNT;
END;
GO
