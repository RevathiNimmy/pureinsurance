SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_SIR_IsMarkedForCollection'
GO
CREATE PROCEDURE spu_SIR_IsMarkedForCollection  
  
@insurance_file_cnt int
  
AS  
  
BEGIN  
  
SELECT marked_for_collection,marked_date from insurance_file  
WHERE insurance_file_cnt =@insurance_file_cnt  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

