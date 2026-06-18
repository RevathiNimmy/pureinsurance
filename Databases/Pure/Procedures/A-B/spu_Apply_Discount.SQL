SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Apply_Discount'
GO

CREATE PROCEDURE spu_Apply_Discount
    @insurance_file_cnt int
AS

-- ******************************************************************************************************
-- Stored Procedure spu_Apply_Discount
-- ******************************************************************************************************
-- Revision             Description of Modification                                     Date        Who 
-- --------             ---------------------------                                     ----        ---
-- 1.0                  RKS 15092005 - Created                                
-- 1.1                  RKS 03112005 PN24662 - this_discount and this_premium will 
--                      only be calculated if there is a change in (calculateddiscount%                  
-- ******************************************************************************************************

BEGIN

    UPDATE rsec 
        SET rsec.applied_discount = isnull(rsec.this_discount,0),
            rsec.this_premium =  isnull(rsec.this_premium,0) + isnull(rsec.this_discount,0)
    FROM Risk rsk
        JOIN insurance_file_risk_link ifrl 
            ON rsk.risk_cnt=ifrl.risk_cnt
        JOIN insurance_file inf 
            ON ifrl.insurance_file_cnt = inf.insurance_file_cnt
        JOIN risk_status rs 
            ON rs.risk_status_id=rsk.risk_status_id 
        JOIN rating_section rsec 
            ON rsk.risk_cnt=rsec.risk_cnt
    WHERE INF.insurance_file_cnt=@insurance_file_cnt
        AND rs.code = 'DISCOUNT'

    UPDATE rsk 
        SET rsk.risk_status_id = rsk.original_risk_status_id  ,
            rsk.original_risk_status_id = NULL
    FROM risk rsk
        JOIN insurance_file_risk_link ifrl 
            ON rsk.risk_cnt=ifrl.risk_cnt
        JOIN insurance_file inf 
            ON ifrl.insurance_file_cnt = inf.insurance_file_cnt
        JOIN risk_status rs 
            ON rs.risk_status_id=rsk.risk_status_id 
        JOIN rating_section rsec 
            ON rsk.risk_cnt=rsec.risk_cnt
    WHERE INF.insurance_file_cnt = @insurance_file_cnt
        AND rs.code = 'DISCOUNT'
    
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
