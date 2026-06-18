SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_PFGetTransactionID'
GO
CREATE PROCEDURE spu_PFGetTransactionID  
    @nPremiumFinanceCnt INT,  
    @nPremiumFinanceversion INT  
  
    AS BEGIN  
     
        SELECT PF.plantransaction_id, TD.insurance_ref_index, TD.amount, PF.insurance_file_cnt  
        FROM PFPremiumFinance AS PF 
        INNER JOIN TransDetail AS TD ON PF.plantransaction_id = TD.transdetail_id  
 		AND PF.PFPrem_Finance_Cnt = @nPremiumFinanceCnt  
 		AND PF.PFPrem_Finance_Version = @nPremiumFinanceversion  
    END  
GO


