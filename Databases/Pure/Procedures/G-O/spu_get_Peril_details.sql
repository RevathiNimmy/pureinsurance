SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_Peril_details'
GO

CREATE PROCEDURE spu_get_Peril_details    
    @Claim_id int    
AS    
    
--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
--    
--*******************************************************************************************    
    


    SELECT  cp.Claim_Peril_id,    
            cp.Peril_type_id,    
            pt.description,    
            cp.Description,    
            claim.currency_id,    
            currency.description,    
            ifi.source_id,    
            cob.class_of_business_id,    
            cob.code    
    FROM    Claim_Peril cp    
    JOIN    Peril_Type pt    
            ON cp.Peril_type_id = pt.peril_type_id    
    JOIN    claim     
            ON claim.claim_id = cp.claim_id    
    JOIN    Currency    
            ON currency.currency_id = claim.currency_id    
    JOIN    Insurance_file ifi    
            ON ifi.insurance_file_cnt = claim.policy_id    
    JOIN    Class_of_Business cob    
            ON cob.class_of_business_id = pt.class_of_business_id    
    WHERE   cp.Claim_id = @Claim_id    
    AND     pt.is_levy_tax <> 1	


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
