SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_Next_CashDeposit_Number'
GO

--Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
CREATE  PROCEDURE spu_Get_Next_CashDeposit_Number 
	@Party_ID INT ,
	@NextNumber INT OUTPUT
AS 
BEGIN
	DECLARE @MaxNumber INT
	SET @MaxNumber=0

	SELECT 
		@MaxNumber=Next_Number
	FROM 
		CashDepositNumber CDN
	WHERE
		CDN.Party_ID=@Party_ID

	SET @NextNumber=@MaxNumber+1

END
--End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

