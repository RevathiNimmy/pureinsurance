SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFInstalments_update_status_to_onhold'
GO


CREATE PROCEDURE spu_PFInstalments_update_status_to_onhold  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int  
  
AS BEGIN  
  
 update pfinstalments  
 set status = 7  
 where pfprem_finance_cnt = @pfprem_finance_cnt  
 and pfprem_finance_version = @pfprem_finance_version  
  
END  





GO
