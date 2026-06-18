
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PT_RI_Usage_upd'
GO

CREATE PROCEDURE spu_SAM_PT_RI_Usage_upd  
(  
 @insurance_file_cnt       int,  
 @PT_RI_status_type_id     int  
)  
AS  
  
UPDATE  
 Insurance_File_PT_RI_Usage  
SET  
 status = @PT_RI_status_type_id  
WHERE  
 insurance_file_cnt = @insurance_file_cnt
