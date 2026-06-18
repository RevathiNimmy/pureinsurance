SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Stats_Folder'
GO

CREATE PROCEDURE spu_CLM_Get_Stats_Folder

@claim_id int, 
@transaction_type_code char(10)=  '' 

AS

BEGIN
	SELECT stats_folder_cnt
	FROM stats_folder sf
	LEFT JOIN Transaction_Export_Folder tef ON
	sf.document_ref = tef.document_ref
	AND tef.document_ref IS NULL  
	WHERE sf.loss_id = @claim_id
	AND (@transaction_type_code = '' OR sf.transaction_type_code = @transaction_type_code)
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
