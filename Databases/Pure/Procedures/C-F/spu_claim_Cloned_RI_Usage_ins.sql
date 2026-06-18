SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_claim_Cloned_RI_Usage_ins'
GO

CREATE PROCEDURE spu_claim_Cloned_RI_Usage_ins
(  
 @old_insurance_file_cnt				INT,
 @new_insurance_file_cnt				INT,
 @old_risk_cnt							INT,
 @new_risk_cnt							INT,
 @claim_cloned_RI_usage_id				INT OUTPUT  
)  
AS  
  
-- get the id for the manual review status  
  
-- check that our record doesn't exist already, dupes would be 'bad'  
IF NOT EXISTS  
    (  
    SELECT  
        claim_cloned_RI_usage_id  
    FROM  
        claim_cloned_RI_Usage  
    WHERE  
        old_insurance_file_cnt = @old_insurance_file_cnt  and
        new_insurance_file_cnt = @new_insurance_file_cnt  and
		old_risk_cnt = @old_risk_cnt and
        new_risk_cnt = @new_risk_cnt
    )  
BEGIN  
    INSERT INTO  
     claim_cloned_RI_Usage  
    (  
     old_insurance_file_cnt,  
     new_insurance_file_cnt,
     old_risk_cnt,New_risk_cnt,  
     status 
    )  
    VALUES  
    (  
     @old_insurance_file_cnt,  
     @new_insurance_file_cnt,
     @old_risk_cnt,@New_risk_cnt,  
     1
    )  
END  
SELECT @claim_cloned_RI_usage_id = @@IDENTITY  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO