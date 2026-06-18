SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Ins_File_Public_Text_dar'
GO

CREATE PROCEDURE spe_Ins_File_Public_Text_dar
 @insurance_file_cnt int
AS
DELETE
FROM Ins_File_Public_Text
WHERE insurance_file_cnt = @insurance_file_cnt

GO

