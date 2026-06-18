SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_CDReceipts_For_Refund'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_CDReceipts_For_Refund
	@CashDeposit_ID INT,
	@Total_Premium MONEY
AS 
BEGIN
	SELECT 
		CDT.Account_ID,
		(SELECT TOP 1
			 TDL.TransDetail_ID
		 FROM
			 TransDetail TDL
			 INNER JOIN Document DOC
				 ON DOC.Document_ID=TDL.Document_ID
			 INNER JOIN DocumentType DMT
				 ON DMT.DocumentType_ID=DOC.DocumentType_ID
		 WHERE
			 TDL.Account_ID=CDT.Account_ID
			 AND DMT.Code='SRP'
		 ORDER BY TransDetail_ID DESC
		) AS TransDetail_ID,	
		@Total_Premium AS Amount_Allocated
	FROM
		CashDeposit CDT
	WHERE
		CDT.CashDeposit_ID=@CashDeposit_ID
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
