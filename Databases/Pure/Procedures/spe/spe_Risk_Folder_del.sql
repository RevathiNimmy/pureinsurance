SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Folder_del'
GO

CREATE PROCEDURE spe_Risk_Folder_del
    @risk_folder_cnt int
AS

DELETE FROM Risk_Folder
WHERE risk_folder_cnt = @risk_folder_cnt

GO

