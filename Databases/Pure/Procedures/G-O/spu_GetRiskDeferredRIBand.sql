SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetRiskDeferredRIBand'
GO

CREATE PROCEDURE spu_GetRiskDeferredRIBand    
 @RiskCnt int    
AS    
    
-- how many bands do we have on deferred ri model    
SELECT  count(*)    
FROM ri_arrangement rra JOIN ri_model rm ON rra.ri_model_id = rm.ri_model_id    
WHERE  rra.risk_cnt = @RiskCnt    
AND  rra.original_flag = 0    
AND  rm.ri_model_type = 2 -- deferred  
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
