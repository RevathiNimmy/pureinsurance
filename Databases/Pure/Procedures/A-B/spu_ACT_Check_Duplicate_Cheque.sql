SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Duplicate_Cheque'
GO


CREATE PROCEDURE spu_ACT_Check_Duplicate_Cheque
    @bankaccount_id int,
    @cheque_number varchar(30)
AS
SELECT c.cheque_id FROM Cheque c JOIN BankAccount b ON
		c.bankaccount_id=b.account_id 
		WHERE b.bankaccount_id=@bankaccount_id 
		AND ISNUMERIC(c.media_ref) = 1		
		AND cast(c.media_ref as int) = cast(@cheque_number as bigint) 
		AND printed_date IS NOT NULL

GO
