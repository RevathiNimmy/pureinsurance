SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claims_Stats_Folders'
GO

CREATE PROCEDURE spu_CLM_Get_Claims_Stats_Folders

@claim_id int 


AS

BEGIN
	SELECT sf.stats_folder_cnt
	FROM Stats_Folder sf 
		LEFT JOIN Transaction_Export_Folder tef ON 
			sf.document_ref = tef.document_ref
			AND tef.document_ref IS NULL
	WHERE sf.loss_id = @claim_id
	
END 


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
