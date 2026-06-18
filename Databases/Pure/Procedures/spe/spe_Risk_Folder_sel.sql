SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Folder_sel'
GO

CREATE PROCEDURE spe_Risk_Folder_sel
    @risk_folder_cnt int
AS

SELECT
    risk_folder_cnt,
    risk_folder_id,
    source_id,
    risk_folder_type_id,
    code,
    description,
    insurance_folder_cnt
FROM Risk_Folder
WHERE risk_folder_cnt = @risk_folder_cnt

GO

