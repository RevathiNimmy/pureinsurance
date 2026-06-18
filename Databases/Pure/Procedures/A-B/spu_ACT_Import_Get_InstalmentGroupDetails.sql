SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Get_InstalmentGroupDetails'
GO


CREATE PROCEDURE spu_ACT_Import_Get_InstalmentGroupDetails  
  
@group_id int OUTPUT, 
@count int OUTPUT
  
AS  
  
BEGIN  
  
 IF NOT EXISTS(SELECT NULL 
	   FROM pfinstalments     
		INNER JOIN pfpremiumfinance ON   
			pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt  
	       	    AND pfinstalments.pfprem_finance_version  = pfpremiumfinance.pfprem_finance_version  
  
	    WHERE group_id = @group_id)

	Set @group_id =0 
  ELSE
	SELECT @count = count(*) 
	FROM pfinstalments 
	WHERE status IN (SELECT pfinstalments_status_id 
			     FROM pfinstalments_status 
			     WHERE code IN ('P', 'C'))
	AND group_id = @group_id

	  
END   






GO
