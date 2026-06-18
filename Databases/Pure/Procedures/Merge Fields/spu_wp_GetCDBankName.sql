

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

EXEC DDLDropProcedure 'spu_wp_GetCDBankName'
GO
--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE PROCEDURE spu_wp_GetCDBankName 
	@PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS

SELECT DISTINCT  
		CLB.Description,
		CLI.bank_branch
		FROM CashListItem_Bank CLB  
        INNER JOIN CashListItem CLI ON CLI.CashlistItem_Bank_Id=CLB.CashListItem_Bank_Id  
        INNER JOIN TransDetail TDL ON TDL.TransDetail_ID=CLI.TransDetail_ID  
        INNER JOIN Document DOC ON DOC.Document_ID=TDL.Document_ID  
        INNER JOIN DocumentType DMT ON DMT.DocumentType_ID=DOC.DocumentType_ID  
		INNER JOIN CashDeposit CDT ON CDT.Account_ID=TDL.Account_ID 
		INNER JOIN CashDeposit_Policy_Link CDPL ON CDT.CashDeposit_ID = CDPL.CashDeposit_ID
		WHERE DMT.Code='SRP' AND CDT.Party_Id = @PartyCnt AND CDPL.Insurance_File_Cnt = @InsuranceFileCnt
		AND CLI.CashlistItem_Bank_Id = @Instance2
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

