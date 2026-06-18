SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Calculate'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Calculate    

    @insurance_file_cnt int,    
    @discount_percentage numeric(11,8)    
AS    
    
-- ******************************************************************************************************    
-- Stored Procedure spu_Calculate_Discount    
-- ******************************************************************************************************    
-- Revision             Description of Modification                                     Date        Who    
-- --------             ---------------------------                                     ----        ---    
-- 1.0                  RKS 15092005 - Created    
-- 1.1                  RKS 03112005 PN24662 - this_discount and this_premium will    
--                      only be calculated if there is a change in (calculateddiscount%)    
-- ******************************************************************************************************    
    
BEGIN    
    
    UPDATE rsec    
            SET rsec.this_premium = ISNULL(rsec.this_premium,0) - ((isnull(rsec.this_premium,0)) * (@discount_percentage/100))  
    FROM Risk rsk    
        JOIN insurance_file_risk_link ifrl    
            ON rsk.risk_cnt=ifrl.risk_cnt    
        JOIN insurance_file inf    
            ON ifrl.insurance_file_cnt = inf.insurance_file_cnt    
        JOIN risk_status rs    
            ON  rs.risk_status_id=rsk.risk_status_id    
        JOIN rating_section rsec    
            ON rsk.risk_cnt=rsec.risk_cnt    
    WHERE inf.insurance_file_cnt=@insurance_file_cnt    
        AND rs.code <> 'UNQUOTED'    
        AND NOT isnull(rsec.this_discount,0)= (isnull(rsec.this_premium,0) - isnull(rsec.applied_discount,0) - isnull(rsec.adjusted_discount,0)) * (@discount_percentage/100)      
    
END    
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
