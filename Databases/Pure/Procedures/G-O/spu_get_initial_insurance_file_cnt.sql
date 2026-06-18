
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_initial_insurance_file_cnt'
GO

CREATE PROCEDURE spu_get_initial_insurance_file_cnt
 @insurance_ref varchar(30),
 @insurance_folder_cnt int
AS

BEGIN

IF EXISTS(SELECT NULL FROM insurance_file ifi WITH(NOLOCK)  
WHERE insurance_ref = @insurance_ref AND insurance_folder_cnt = @insurance_folder_cnt  AND insurance_file_cnt = (SELECT MAX(ifi1.insurance_file_cnt) FROM insurance_file ifi1 WITH(NOLOCK) WHERE ifi1.insurance_folder_cnt =  @insurance_folder_cnt))
 
	SELECT Top 1    
	 insurance_file_cnt    
	FROM    
	 insurance_file ifi WITH(NOLOCK)    
	WHERE    
	 insurance_ref = @insurance_ref    
	AND    
	 insurance_folder_cnt = @insurance_folder_cnt    
	AND insurance_file_cnt = (SELECT MAX(ifi1.insurance_file_cnt) FROM    
	 insurance_file ifi1 WITH(NOLOCK)    
	 WHERE ifi1.insurance_folder_cnt =  @insurance_folder_cnt)    
	ORDER BY    
	 policy_version desc
 
ELSE
 
	SELECT Top 1    
	 insurance_file_cnt    
	FROM    
	 insurance_file ifi WITH(NOLOCK)    
	WHERE    
	 insurance_ref = @insurance_ref    
	AND    
	 insurance_folder_cnt = @insurance_folder_cnt    
	ORDER BY    
	 policy_version desc

END  
GO

