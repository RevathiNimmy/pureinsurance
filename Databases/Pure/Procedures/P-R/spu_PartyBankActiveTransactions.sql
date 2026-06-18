SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBankActiveTransactions'
GO

CREATE PROCEDURE spu_PartyBankActiveTransactions    
    @Party_Bank_Id INT,    
    @ActiveTransExists INT = NULL OUTPUT    
    
AS    
    
    SELECT  @ActiveTransExists = COUNT(pfprem_finance_cnt)    
        FROM pfpremiumfinance PF    
    WHERE  Party_Bank_Id = @Party_Bank_Id    
        AND  PF.StatusInd IN ('040','140')    
  
    IF @ActiveTransExists=0      
        SELECT  @ActiveTransExists = COUNT(cashlist_id)    
        FROM CashListItem    
        WHERE  Party_Bank_Id = @Party_Bank_Id    
   
    IF @ActiveTransExists=0    
        SELECT  @ActiveTransExists = COUNT(Claim_Payment_id)    
        FROM Claim_Payment    
        WHERE  Party_Bank_Id = @Party_Bank_Id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO