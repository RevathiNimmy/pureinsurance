SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurance_Folder_del'
GO

CREATE PROCEDURE spe_Insurance_Folder_del
    @insurance_folder_cnt int
AS
DELETE FROM Insurance_Folder
WHERE insurance_folder_cnt = @insurance_folder_cnt

GO

