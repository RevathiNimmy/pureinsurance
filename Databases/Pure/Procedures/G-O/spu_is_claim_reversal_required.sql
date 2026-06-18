SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_is_claim_reversal_required'
GO


CREATE PROCEDURE spu_is_claim_reversal_required  
    @claim_id             INT,  
    @reversal_required    INT = 0 OUTPUT  
AS  
  
DECLARE @Base_claim_id    INT,  
        @last_claim_id    INT,  
        @last_version_id    INT,  
        @version_id    INT,  
        @this_reserve    NUMERIC(19,4)  
  
  
    SELECT  @this_reserve = this_reserve   
    FROM claim_ri_arrangement  
    WHERE claim_id = @claim_id  
  
    IF @this_reserve = 0  
        BEGIN  
            Select @Base_claim_id = base_claim_id from claim where Claim_id = @Claim_id  
          
            SELECT  @last_claim_id =MAX(claim_id) from claim  
            WHERE base_claim_id = @Base_claim_id  
            AND Claim_id < @Claim_id  
          
            SELECT  @last_version_id = ri_arrangement_version from claim_ri_arrangement  
            WHERE claim_id = @last_claim_id  
          
            SELECT  @version_id = ri_arrangement_version from claim_ri_arrangement  
            WHERE claim_id = @claim_id  
              
            IF @last_version_id <> @version_id  
                SET @reversal_required = 1  
            ELSE IF @last_version_id <> @version_id  
                SET @reversal_required = 0  
        END  


SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO
