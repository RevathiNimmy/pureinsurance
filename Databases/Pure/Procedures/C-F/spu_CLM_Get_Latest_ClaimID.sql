SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_Latest_ClaimID'
GO
CREATE PROCEDURE spu_CLM_Get_Latest_ClaimID  
  
@claim_number varchar(30)  
  
AS  
  
BEGIN  
  
 SELECT MAX(claim_id)  
 FROM Claim  
 WHERE Claim_Number = @claim_number and is_dirty = 0  
  
END  

GO