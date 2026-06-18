SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_Linked_CashDeposit_Account_IDs'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_Linked_CashDeposit_Account_IDs
	@Account_ID INT
AS
BEGIN
	SELECT DISTINCT
		CDT.Account_ID
	FROM 
		CashDeposit CDT
	WHERE
		CDT.Party_ID=(
					  SELECT
						  Account_Key
					  FROM
						  Account
					  WHERE
						  Account_ID=@Account_ID
					 )
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
