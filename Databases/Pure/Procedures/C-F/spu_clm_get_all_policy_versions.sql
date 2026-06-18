SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_get_all_policy_versions'
GO


CREATE PROCEDURE spu_clm_get_all_policy_versions

@insurance_file_cnt int

AS

BEGIN

	DECLARE @insurance_folder_cnt int
	DECLARE @policy_version int	

	SELECT @insurance_folder_cnt =insurance_Folder_cnt, @policy_version = policy_version 
	from insurance_file 
	where  insurance_file_cnt = @insurance_file_cnt

	SELECT insurance_file_cnt, insurance_file_type_id, insurance_file_status_id, policy_version, cover_start_date 
	FROM insurance_file
	WHERE insurance_folder_cnt = @insurance_folder_cnt
	AND policy_version >= @policy_version  AND insurance_file_cnt>=@insurance_file_cnt
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
