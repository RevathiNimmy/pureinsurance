SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDROPPROCEDURE 'spu_SAM_Delete_Original_Risk_Reinsurance'
GO  

CREATE PROCEDURE spu_SAM_Delete_Original_Risk_Reinsurance  
    @risk_cnt int  
AS  
  
  DELETE FROM RI_Arrangement_line_Broker_Participants WHERE Ri_arrangement_line_id In  
  (SELECT ri_arrangement_line_id FROM  
 ri_arrangement_line AS rial  
INNER JOIN ri_arrangement ria  
 ON rial.ri_arrangement_id = ria.ri_arrangement_id  
WHERE  ria.risk_cnt = @risk_cnt  and  ria.original_flag=1)  
  
DELETE ri_arrangement_line FROM  
 ri_arrangement_line AS rial  
INNER JOIN ri_arrangement ria  
 ON rial.ri_arrangement_id = ria.ri_arrangement_id  
WHERE  
 ria.risk_cnt = @risk_cnt  
 and  ria.original_flag=1
 
DELETE FROM  
 ri_arrangement  
WHERE  
 risk_cnt = @risk_cnt and original_flag=1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


