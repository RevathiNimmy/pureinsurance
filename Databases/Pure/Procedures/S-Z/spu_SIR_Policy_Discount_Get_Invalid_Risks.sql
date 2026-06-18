SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Get_Invalid_Risks'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Get_Invalid_Risks    
    
@insurance_file_cnt int    
    
AS    
    
BEGIN    
    
 SELECT ifrl.risk_cnt,    
  ROUND(r.premium_this_year,2),    
  ROUND(r.total_this_premium,2),    
 rt.description,    
 r.risk_number    
 FROM insurance_file_risk_link ifrl    
 INNER JOIN risk r ON    
  ifrl.risk_cnt = r.risk_cnt    
 INNER JOIN risk_type rt ON    
  r.risk_type_id = rt.risk_type_id    
 INNER JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES rs ON    
 r.risk_status_id = rs.risk_status_id    
 WHERE ifrl.insurance_file_cnt = @insurance_file_cnt    
 AND r.is_risk_selected = 1    
 AND ROUND(ISNULL(r.total_this_premium,0),2) + ROUND(ISNULL(r.premium_this_year,0),2) < 0    
    
END    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
