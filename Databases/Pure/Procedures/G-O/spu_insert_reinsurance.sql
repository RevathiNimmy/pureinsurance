SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_insert_reinsurance'
GO

CREATE PROCEDURE spu_insert_reinsurance  
    @ClaimID INT,  
    @PartyID numeric,  
    @Share numeric(12,8),  
    @Share_Value numeric(19,4)  
AS  
  
BEGIN
    Insert Into Claim_Party (Claim_id,Party_id,Share,Share_Value,insurer_type) Values  
         (@ClaimID,@PartyID,@Share,@Share_Value,1)  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
