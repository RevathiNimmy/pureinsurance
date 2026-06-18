SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_batchprocesses_sel'
GO

CREATE PROCEDURE spu_sir_batchprocesses_sel  
 @batchprocesses_list_id INT
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 IF @batchprocesses_list_id = 0
 BEGIN
	 SELECT batchprocesses_list_id AS BatchProcessId,description AS Description 
	 FROM  BatchProcesses_List 
	 ORDER BY description
 END
 ELSE
 BEGIN
	 SELECT batchprocesses_list_id AS BatchProcessId,a.description 	AS Description,b.component_class_name,b.component_object_name 
	 FROM  BatchProcesses_List a
	 JOIN PMWrk_Task b on a.pmwrk_task_id=b.pmwrk_task_id
	 WHERE a.batchprocesses_list_id=@batchprocesses_list_id
 END

END  