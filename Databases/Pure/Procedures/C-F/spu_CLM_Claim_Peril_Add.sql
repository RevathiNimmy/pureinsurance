SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Peril_Add'
GO


CREATE PROCEDURE spu_CLM_Claim_Peril_Add  
    @claim_id int,  
    @peril_type_id int,  
    @description varchar(50) = Null,  
    @comments varchar(255) = Null,  
    @sum_insured numeric(19, 4) = Null,  
    @ri_band int = Null,  
    @claim_peril_id int OUTPUT  
AS  
  
     -- ensure a claim peril cnt is set against the item  
    INSERT INTO Claim_Peril  
        (  
        claim_id,  
        peril_type_id,  
        description,  
        comments,  
        sum_insured,  
        ri_band	
        )  
    VALUES  
        (  
        @claim_id,  
        @peril_type_id,  
        @description,  
        @comments,  
        @sum_insured,  
        @ri_band	 
        )  
  
IF @@ERROR <> 0  
    BEGIN  
        GOTO Error_Routine  
    END  

SELECT @claim_peril_id = @@IDENTITY  

UPDATE claim_peril 
SET base_claim_peril_id = @claim_peril_id, 
     version_id = claim.version_id
FROM claim_peril 
	INNER JOIN Claim ON 
		claim_peril.claim_id = claim.claim_id
WHERE claim_peril_id = @claim_peril_id

RETURN  
Error_Routine:  
SELECT @Claim_peril_id = 0  
RETURN  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
