SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_cloned_Usage_upd'
GO

CREATE PROCEDURE spu_cloned_Usage_upd  
(  
 @ins_file_cloned_RI_usage_id  int,  
 @insurance_file_cnt           int,  
 @cloned_RI_status_type_id     int  
)  
AS  
  
UPDATE  
 Insurance_File_cloned_RI_Usage  
SET  
 new_insurance_file_cnt = @insurance_file_cnt,  
 status = @cloned_RI_status_type_id  
WHERE  
 ins_file_cloned_RI_usage_id = @ins_file_cloned_RI_usage_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 