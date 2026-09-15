USE LegacyBankDataCore;
GO

CREATE OR ALTER PROCEDURE dbo.SP_IMPORTACAO_REGISTRAR_ERRO
    @Id INT,
    @MensagemErro VARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.IMPORTACAO
    SET
        Status = 'ERRO',
        DataProcessamento = SYSUTCDATETIME(),
        MensagemErro = @MensagemErro
    WHERE
        Id = @Id
        AND Status = 'PROCESSANDO';

    SELECT @@ROWCOUNT;
END;
GO
