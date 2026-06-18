SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Safe_Harbour_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Safe_Harbour_Details  
  
AS  
  
BEGIN  
 SELECT safe_harbour_id, description, code,  percentage  
 FROM safe_harbour  
 WHERE is_deleted = 0  
 AND effective_date <= GetDate()  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
