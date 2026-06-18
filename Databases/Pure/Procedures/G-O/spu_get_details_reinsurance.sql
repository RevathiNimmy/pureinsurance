SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_details_reinsurance'
GO

CREATE PROCEDURE spu_get_details_reinsurance  
    @ClaimID INT,  
    @PartyID numeric  
AS  
  
BEGIN  
    	SELECT 	
		Share,
	   	Share_Value 
	FROM Claim_Party 
	WHERE Claim_id = @ClaimID 
	AND Party_id =@PartyID  
        AND insurer_type = 1  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
