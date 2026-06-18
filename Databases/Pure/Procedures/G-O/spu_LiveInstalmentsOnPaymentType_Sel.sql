
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_LiveInstalmentsOnPaymentType_Sel'
GO

CREATE PROCEDURE spu_LiveInstalmentsOnPaymentType_Sel
	@Party_cnt		INT,
	@Party_Bank_Id	INT,
	@IsExitsInstalments	INT = NULL OUTPUT

AS


        SELECT 	@IsExitsInstalments = COUNT(pfprem_finance_cnt)
        	FROM pfpremiumfinance PF
        WHERE 	clientid = @Party_cnt
        AND	Party_Bank_Id = @Party_Bank_Id
        AND 	PF.StatusInd IN ('040','140')
	IF @IsExitsInstalments=0
	BEGIN
		SELECT 	@IsExitsInstalments = COUNT(cashlist_id)
	        	FROM CashListItem 
	        WHERE 	Party_Bank_Id = @Party_Bank_Id
	END
	IF @IsExitsInstalments=0
	BEGIN
		SELECT 	@IsExitsInstalments = COUNT(Claim_Payment_id)
	        	FROM Claim_Payment 
	        WHERE 	Party_Bank_Id = @Party_Bank_Id
	END	

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

