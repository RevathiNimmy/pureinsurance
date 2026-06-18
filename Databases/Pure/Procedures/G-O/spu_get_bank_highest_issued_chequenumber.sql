SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_bank_highest_issued_chequenumber'
GO

CREATE PROCEDURE spu_get_bank_highest_issued_chequenumber
	@bankaccount_id int,
	@highest_issued_chequenumber bigint OUTPUT
As
BEGIN
	SELECT @highest_issued_chequenumber=MAX(CAST(media_ref AS bigint)) 
	FROM Cheque 
	WHERE bankaccount_id IN(SELECT account_id 
							FROM BankAccount 
							WHERE bankaccount_Id=@bankaccount_id)
	AND printed_date IS NOT NULL
END
	
GO
