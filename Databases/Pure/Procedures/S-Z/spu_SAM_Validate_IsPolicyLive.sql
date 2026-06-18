SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Validate_IsPolicyLive
GO
CREATE PROCEDURE spu_SAM_Validate_IsPolicyLive  
    @insuranceFileCnt int     
AS  
 BEGIN
 
 SELECT d.document_id  FROM Document d 
 INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt = d.insurance_file_cnt 
 WHERE ifi.insurance_file_cnt = @insuranceFileCnt 
 
 END 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


