SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_cloned_RI_Usage_del'
GO

CREATE PROCEDURE spu_Insurance_File_cloned_RI_Usage_del  
(  
 @ins_file_cloned_RI_usage_id int  
)  
AS  
  
DELETE FROM  
 Insurance_File_cloned_RI_Usage  
WHERE  
 ins_file_cloned_RI_usage_id = @ins_file_cloned_RI_usage_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO