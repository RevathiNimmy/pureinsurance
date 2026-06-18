SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Update_CashDeposit_Policy_Link'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Update_CashDeposit_Policy_Link 
	@Insurance_File_Cnt INT,
	@CashDeposit_Account_ID INT ,
	@CashDepsoit_ID INT OUT
AS 
BEGIN

	SELECT 
		@CashDepsoit_ID =CDT.CashDeposit_ID
	FROM 
		CashDeposit CDT
	WHERE
		CDT.Account_ID=@CashDeposit_Account_ID

	IF ISNULL(@CashDepsoit_ID,0)<>0
	BEGIN
		INSERT INTO
			CashDeposit_Policy_Link(
				Insurance_File_Cnt,
				CashDeposit_ID)
		VALUES(
			@Insurance_File_Cnt,
			@CashDepsoit_ID)
	END										
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

