SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Copy_Risk_Billed_Premium'
GO

CREATE PROCEDURE spu_SIR_Copy_Risk_Billed_Premium  
  
@new_risk_cnt int,   
@old_risk_cnt int  
  
AS  
  
BEGIN  
  
 UPDATE risk SET premium_this_year = (
	SELECT premium_this_year 
	FROM risk 
	WHERE risk_cnt = @old_risk_cnt)  
 WHERE risk_cnt =  @new_risk_cnt    
    
END  
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
