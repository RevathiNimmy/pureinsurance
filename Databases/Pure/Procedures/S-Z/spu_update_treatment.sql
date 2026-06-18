SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_update_treatment'
GO

CREATE PROCEDURE spu_update_treatment  
    @ClaimID int,  
    @Description numeric  
AS  
  
BEGIN
     Update Claim  
        Set Coinsurance_Treatment_id=@description  
        where Claim_id=@ClaimID  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
