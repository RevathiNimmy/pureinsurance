SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Stats_Folder_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Stats_Folder_Details

@stats_folder_cnt int 

AS

BEGIN
	SELECT 
		document_ref, 
		document_comment, 
		document_date, 
		insurance_ref
	FROM Stats_Folder 
	WHERE stats_folder_cnt = @stats_folder_cnt
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
