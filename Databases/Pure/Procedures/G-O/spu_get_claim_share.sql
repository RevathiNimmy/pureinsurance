SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claim_share'
GO

CREATE PROCEDURE spu_get_claim_share  
    @ClaimID numeric  
AS  
  
BEGIN  
    SELECT Claim_Party.Share
    FROM Claim_Party,  Party 
    WHERE Claim_Party.Claim_id=@ClaimID  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
