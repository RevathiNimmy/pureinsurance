SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_check_bankaccount_mapping'
GO


CREATE PROCEDURE spu_ACT_check_bankaccount_mapping
    @account_id INT,
    @bankaccount_id INT
AS


SELECT  b.account_id 
FROM    bankaccount b 
WHERE   b.account_id = @account_id
AND     (@bankaccount_id = 0
	or b.bankaccount_id <> @bankaccount_id
	)
GO