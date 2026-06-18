SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PFInstalments_History_Select'
GO

CREATE PROCEDURE spu_ACT_PFInstalments_History_Select  
  
@pfinstalments_id int  
  
AS  
  
BEGIN  
  
 SELECT  
  posted_date,  
  pfinstalments_status.description,  
  pfinstalments_result.description,  
  pfinstalments_result.code  
  
 FROM PFInstalments_History  
  
 LEFT JOIN pfinstalments_status ON  
  pfinstalments_status.pfinstalments_status_id = pfinstalments_history.pfinstalments_status_id  
  
 LEFT JOIN pfinstalments_result ON  
  pfinstalments_result.pfinstalments_result_id = pfinstalments_history.pfinstalments_result_id  
  
 WHERE pfinstalments_id = @pfinstalments_id  
  
END  



GO
