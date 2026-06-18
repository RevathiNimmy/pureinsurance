SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Add_PFInstalments_History_For_Group'
GO

CREATE PROCEDURE spu_ACT_Add_PFInstalments_History_For_Group  
  
@group_id int  
  
AS  
  
BEGIN  
  
 INSERT INTO PFInstalments_History  
 (pfinstalments_id,  
  posted_date,  
  pfinstalments_status_id,  
  pfinstalments_result_id)  
  
 SELECT  
  pfinstalments_id,  
  posteddate,  
  status,  
  pfinstalments_result_id  
 FROM pfinstalments 

 WHERE group_id = @group_id
  
END  



GO
