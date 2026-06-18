SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_risk_status_sel2'
GO

CREATE PROCEDURE spu_CLM_risk_status_sel2  
    @claim_id int  
AS  
  
SELECT  
    r.risk_status_id,  
 rs.code  
FROM  
 claim c  
    INNER JOIN Risk AS r ON r.risk_cnt = c.risk_type_id  
 INNER JOIN Risk_Status AS rs ON rs.risk_status_id = r.risk_status_id  
WHERE  
    c.claim_id = @claim_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
