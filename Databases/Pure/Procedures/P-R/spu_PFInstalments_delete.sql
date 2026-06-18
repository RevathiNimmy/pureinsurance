SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFInstalments_Delete'
GO

CREATE PROCEDURE spu_PFInstalments_Delete  
    @finance_cnt int,  
    @finance_version int  
  
AS BEGIN  

    DELETE FROM PFInstalments_History 
    WHERE pfinstalments_id in (Select pfinstalments_id from pfinstalments
    WHERE pfprem_finance_cnt=@finance_cnt  
    AND pfprem_finance_version=@finance_version)

  
    DELETE FROM PFInstalments  
    WHERE pfprem_finance_cnt=@finance_cnt  
    AND pfprem_finance_version=@finance_version  


END  



GO
