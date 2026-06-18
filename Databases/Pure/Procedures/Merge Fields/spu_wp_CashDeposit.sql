
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

EXEC DDLDropProcedure 'spu_wp_CashDeposit'
GO
--Start - Renuka - (WPR85_Cash_Deposit_Process)
 CREATE PROCEDURE spu_wp_CashDeposit  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
  
AS  
  
 SELECT CashDeposit.CashDeposit_Ref,
      (SELECT
			 SUM(outstanding_amount)*-1
		 FROM 
			TransDetail TDT
			INNER JOIN [Document] DOC ON DOC.Document_ID=TDT.Document_ID
			INNER JOIN DocumentType DMT ON DMT.DocumentType_ID=DOC.DocumentType_ID
			WHERE
				 TDT.Account_ID=CashDeposit.Account_ID
				 AND DMT.Code='SRP')  as CDBalance
 FROM CashDeposit 
	INNER JOIN Party ON Party.Party_cnt=CashDeposit.Party_ID
	INNER JOIN CashDeposit_Policy_Link CDPL ON CashDeposit.CashDeposit_ID = CDPL.CashDeposit_ID
 WHERE CashDeposit.Party_ID= @PartyCnt AND CDPL.Insurance_File_Cnt = @InsuranceFileCnt
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO