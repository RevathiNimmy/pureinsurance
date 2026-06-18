SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Account_UnallocatedCreditAmount'
GO

CREATE PROCEDURE spu_ACT_Get_Account_UnallocatedCreditAmount  
  
 @account_id int  
  
AS  
  
BEGIN  
  
 -- returns the sum of all unallocated  
 -- credits on the specified account  
 SELECT ISNULL(SUM(outstanding_amount),0) 
 FROM transdetail  
 WHERE account_id = @account_id  
 AND fully_matched = 0  
 AND amount < 0  
  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
