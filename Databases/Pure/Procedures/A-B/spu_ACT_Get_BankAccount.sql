SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_BankAccount'
GO

CREATE PROCEDURE spu_ACT_Get_BankAccount
    @bank_id int
AS

    SELECT  	ba.bankaccount_id, 
            	ba.code, 
            	ba.bank_account_no, 
            	ba.bank_account_name, 
            	ba.description, 
            	c.code company, 
            	sb.code sub_branch,
            	c.company_id
    FROM    	bankaccount ba
    JOIN    	company c ON c.company_id = ba.company_id 
    LEFT JOIN   sub_branch sb ON sb.sub_branch_id = ba.sub_branch_id
    WHERE   	bank_id = @bank_id

GO



