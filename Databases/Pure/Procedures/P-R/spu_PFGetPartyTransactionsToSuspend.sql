SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_PFGetPartyTransactionsToSuspend'
GO

-- Object:  Stored Procedure dbo.spu_PFGetPartyTransactionsToSuspend    
-- Script Date: 10/21/2003 8:22:48 AM ******/

CREATE PROCEDURE spu_PFGetPartyTransactionsToSuspend    
    @PremiumFinanceCnt int,    
    @PremiumFinanceVersion int,    
    @AccountID int     
AS    


SELECT  TransDetail.Amount as TransAmount,     
  TransDetail.outstanding_currency_amount AS NetTransAmount,     
  TransDetail.spare,    
        TransDetail.transdetail_id     
FROM    Insurance_File with (NoLock)    

INNER JOIN Document with (NoLock) ON Document.insurance_file_cnt = Insurance_File.insurance_file_cnt     
INNER JOIN TransDetail with (NoLock) ON TransDetail.document_id = Document.document_id    
INNER JOIN PFPremiumFinance with (NoLock) ON PFPremiumFinance.Insurance_File_Cnt = Insurance_File.insurance_file_cnt    
WHERE   (PFPremiumFinance.pfprem_finance_cnt = @PremiumFinanceCnt) AND     
  (PFPremiumFinance.pfprem_finance_version = @PremiumFinanceVersion) AND     
  (TransDetail.account_id = @AccountID)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

