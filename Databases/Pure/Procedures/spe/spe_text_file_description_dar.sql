SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_description_dar'
GO

CREATE PROCEDURE spe_text_file_description_dar
AS
DELETE
FROM text_file_description

GO

