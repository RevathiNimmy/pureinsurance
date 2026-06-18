SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_update_risk_values'
GO


CREATE PROCEDURE spu_update_risk_values  
    @risk_cnt INT,  
    @pro_rata_rate float,
    @UpdateRiskToZero int=0  

AS  
  

DECLARE  
    @sum_insured    NUMERIC(19,4),  
    @this_premium   NUMERIC(19,4),  
    @annual_premium NUMERIC(19,4)  
  

SELECT  @sum_insured = ISNULL(SUM(sum_insured), 0)
FROM    rating_section
WHERE   risk_cnt = @risk_cnt
AND     rate_type_id not in (select rate_type_id from rate_type where code = 'Q')
AND     original_flag = 0

SELECT  @this_premium = ISNULL(SUM(p.this_premium), 0)  
-- Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id  
--End - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
WHERE   p.risk_cnt = @risk_cnt  
AND     p.is_premium = 1  
--Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling
AND     (isnull(pt.is_stamp_duty_insurer,0) <> 1 AND isnull(pt.is_stamp_duty_insured,0) <> 1)  
--End - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
-- PWF 09/01/2003 - ISS8211  
-- When getting future annual premium don't include previous return premiums!  
SELECT  @annual_premium = ISNULL(SUM(p.annual_premium), 0)  
--Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
FROM    peril p Join Peril_Type pt ON p.Peril_Type_id= pt.Peril_Type_id  
--End - Sankar - (WR29 - Stamp Duty Process) - Paralleling
JOIN    rating_section rs 
        ON  p.rating_section_id = rs.rating_section_id
        AND p.risk_cnt = rs.risk_cnt
WHERE   p.risk_cnt = @risk_cnt
AND     p.is_premium = 1
--Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling
AND     (isnull(pt.is_stamp_duty_insurer,0) <> 1 AND isnull(pt.is_stamp_duty_insured,0) <> 1)  
--End - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
AND     rs.original_flag = 0  
  
IF @UpdateRiskToZero = 1
BEGIN
    UPDATE risk
    SET total_sum_insured = 0,  
        total_this_premium = 0,  
        total_annual_premium = 0,  
        pro_rata_rate = 1  
    WHERE risk_cnt = @risk_cnt;
END
ELSE IF @UpdateRiskToZero = 2
BEGIN
    UPDATE risk
    SET total_sum_insured = @sum_insured,
        total_this_premium = 0,  
        total_annual_premium = 0  
    WHERE risk_cnt = @risk_cnt;
END
ELSE
BEGIN
    UPDATE risk
    SET total_sum_insured = @sum_insured,  
        total_this_premium = @this_premium,  
        total_annual_premium = @annual_premium,  
        pro_rata_rate = @pro_rata_rate  
    WHERE risk_cnt = @risk_cnt;
END


GO



