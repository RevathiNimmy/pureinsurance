Execute DDLDropProcedure 'spu_CLM_Get_Stats_Folder_For_Claim'
GO

CREATE PROCEDURE spu_CLM_Get_Stats_Folder_For_Claim

@claim_id int, 
@nIgnoreDocRef int=0
AS

BEGIN
	SELECT stats_folder_cnt
	FROM stats_folder
	WHERE loss_id = @claim_id 
	AND( (@nIgnoreDocRef=0 AND document_ref='Doc Ref' ) or (@nIgnoreDocRef=1))
END
