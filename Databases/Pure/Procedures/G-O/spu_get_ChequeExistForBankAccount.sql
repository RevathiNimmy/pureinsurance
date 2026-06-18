SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_ChequeExistForBankAccount'
GO

CREATE  PROCEDURE spu_get_ChequeExistForBankAccount
	@bankaccount_id int
As
BEGIN
	SELECT TOP 1 media_ref FROM Cheque WHERE printed_date IS NOT NULL AND bankaccount_id 
	IN(SELECT account_id FROM BankAccount WHERE bankaccount_Id=@bankaccount_id)
END

GO
