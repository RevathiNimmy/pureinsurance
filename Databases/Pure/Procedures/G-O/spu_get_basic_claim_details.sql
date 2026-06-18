SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_basic_claim_details'
GO

CREATE PROCEDURE spu_get_basic_claim_details  
    @claimId integer  
AS  
  

    select  Client_name,  
        Claim_Number,  
        Progress_Status_Id,  
        Primary_Cause_Id,  
        Secondary_Cause_Id,  
        claim_status_id,  
        description,  
        Policy_Number,
 	Client_short_name 
    from    claim  
    where   Claim_Id = @ClaimID  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
