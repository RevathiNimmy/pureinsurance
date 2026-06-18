SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Update_Risk_Values'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Update_Risk_Values      
    @risk_cnt INT      
AS      
      
BEGIN    
    
DECLARE      
    @sum_insured    NUMERIC(19,4),      
    @this_premium   NUMERIC(19,4),      
    @annual_premium NUMERIC(19,4)      
      
--*******************************************************************************    
    
SELECT  @sum_insured = ISNULL(SUM(sum_insured), 0)      
FROM    rating_section      
WHERE   risk_cnt = @risk_cnt      
AND     rate_type_id not in (select rate_type_id from rate_type where code = 'Q')      
AND     original_flag = 0      
    
--*******************************************************************************    
    
SELECT  @this_premium = ISNULL(SUM(p.this_premium), 0)      
FROM    peril p  with (nolock)   
 JOIN    rating_section rs    with (nolock)   
         ON  p.rating_section_id = rs.rating_section_id      
         AND p.risk_cnt = rs.risk_cnt      
WHERE   rs.risk_cnt = @risk_cnt      
AND     is_premium = 1      
    
--*******************************************************************************    
-- When getting future annual premium don't include previous return premiums!      
    
SELECT  @annual_premium = ISNULL(SUM(p.annual_premium), 0)      
FROM    peril p      
 JOIN    rating_section rs      
         ON  p.rating_section_id = rs.rating_section_id      
         AND p.risk_cnt = rs.risk_cnt      
WHERE   p.risk_cnt = @risk_cnt      
AND     p.is_premium = 1      
AND     rs.original_flag = 0      
    
--*******************************************************************************    
    
UPDATE  risk      
SET     total_sum_insured = @sum_insured,      
        total_this_premium = @this_premium,      
        total_annual_premium = @annual_premium      
WHERE   risk_cnt = @risk_cnt      
    
--******************************************************************************    
    
END    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
