SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_bank_start_chequenumber'
GO

CREATE PROCEDURE spu_get_bank_start_chequenumber
	@bankaccount_id int,
	@start_cheque_number bigint OUTPUT
As
BEGIN
	SELECT @start_cheque_number=start_cheque_number
	FROM BankAccount 
	WHERE bankaccount_id=@bankaccount_id
END
	
GO
