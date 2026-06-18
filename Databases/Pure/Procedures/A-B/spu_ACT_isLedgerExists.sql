SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_isLedgerExists'
GO
CREATE PROCEDURE spu_ACT_isLedgerExists
    @account_id INT
AS
BEGIN
	DECLARE @IsLedgerExist smallint 
	IF EXISTS(	SELECT account_id 
				FROM TransDetail 
				WHERE account_id=@account_id)
		BEGIN
			SET @IsLedgerExist=1
		END

	ELSE
		BEGIN
			SET @IsLedgerExist=0
		END

	SELECT @IsLedgerExist

END
GO


