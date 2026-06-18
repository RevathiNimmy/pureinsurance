SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurance_File_System_del'
GO

CREATE PROCEDURE spe_Insurance_File_System_del
    @insurance_file_cnt int
AS
DELETE FROM Insurance_File_System
WHERE insurance_file_cnt = @insurance_file_cnt

GO

