SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Deferred_RI_Usage_Del_By_InsFileCnt'
GO


CREATE PROCEDURE spu_Insurance_File_Deferred_RI_Usage_Del_By_InsFileCnt  
(  
 @insurance_file_cnt int  
)  
AS  
  
DELETE FROM  
 Insurance_File_Deferred_RI_Usage  
WHERE  
 insurance_file_cnt = @insurance_file_cnt  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
