SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Maximum_PolicyVersion'
GO

CREATE PROCEDURE spu_Get_Maximum_PolicyVersion
	@InsuranceFileCnt	 INT 
AS

	SELECT MAX(ISNULL(ifi.policy_version ,0))
	FROM   
		insurance_file ifi WITH (NOLOCK) 
		INNER JOIN insurance_file ifi2 WITH (NOLOCK)
	ON
		ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt  
	WHERE ifi.policy_ignore IS NULL 
		AND ifi2.insurance_file_cnt =  @InsuranceFileCnt

GO
