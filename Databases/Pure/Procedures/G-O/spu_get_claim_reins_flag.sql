SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claim_reins_flag'
GO

CREATE PROCEDURE spu_get_claim_reins_flag  
    @claim_id int,  
    @is_ri_at_risk_level tinyint OUTPUT  
AS  
  
BEGIN
    SELECT  @is_ri_at_risk_level = is_ri_at_risk_level  
  
    FROM        Risk r,  
            Claim c  
  
    WHERE   c.claim_id = @claim_id  
    AND     c.risk_type_id = r.risk_cnt  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
