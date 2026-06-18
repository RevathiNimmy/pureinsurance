SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_CancellationTransactions_Add'
GO

CREATE PROCEDURE spu_PFPremiumFinance_CancellationTransactions_Add  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int,  
    @transdetail_id int
AS  
BEGIN  
INSERT INTO PFPremiumFinance_Cancellation_Transactions (  
    pfprem_finance_cnt ,  
    pfprem_finance_version,  
    transdetail_id)  
VALUES (  
    @pfprem_finance_cnt,  
    @pfprem_finance_version,  
    @transdetail_id)  
END 
GO
