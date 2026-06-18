SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Adjust_Discount'
GO

CREATE PROCEDURE spu_Adjust_Discount
    @insurance_file_cnt int,
    @adjusted_discount numeric(4,4)
AS

-- ******************************************************************************************************
-- Stored Procedure spu_Adjust_Discount
-- ******************************************************************************************************
-- Revision             Description of Modification                                     Date        Who 
-- --------             ---------------------------                                     ----        ---
-- 1.0                  RKS 15092005 - Created                                
-- ******************************************************************************************************

BEGIN

    DECLARE @max_risk_cnt int
    DECLARE @max_rating_section_id int
        
    
    SELECT @max_risk_cnt = MAX(rsec.risk_cnt),
           @max_rating_section_id = MAX(rsec.rating_section_id)
    FROM risk rsk
        JOIN rating_section rsec
            ON rsk.risk_cnt=rsec.risk_cnt
        JOIN insurance_file_risk_link ifrl 
            ON rsk.risk_cnt = ifrl.risk_cnt
        JOIN insurance_file inf 
            ON ifrl.insurance_file_cnt = inf.insurance_file_cnt
        JOIN risk_status rs 
            ON  rs.risk_status_id=rsk.risk_status_id 
    WHERE inf.insurance_file_cnt=@insurance_file_cnt
    AND rs.code = 'QUOTED'
    
    

    UPDATE rating_section 
        SET adjusted_discount = @adjusted_discount ,
            this_premium = this_premium + @adjusted_discount
    WHERE rating_section_id = @max_rating_section_id 
    AND risk_cnt = @max_risk_cnt  

END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
