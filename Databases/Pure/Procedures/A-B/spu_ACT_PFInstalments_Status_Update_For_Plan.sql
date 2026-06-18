SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PFInstalments_Status_Update_For_Plan'
GO

CREATE PROCEDURE spu_ACT_PFInstalments_Status_Update_For_Plan  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int, 
    @statuscode varchar(10)
  
AS BEGIN  
  
 UPDATE pfinstalments  
 SET status = (SELECT pfinstalments_status_id FROM pfInstalments_status WHERE code = @statuscode)
 WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
 AND pfprem_finance_version = @pfprem_finance_version  
  
END  


GO
