SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_text_file_description_del'
GO


CREATE PROCEDURE spu_text_file_description_del
AS

--  @source_id int
DELETE FROM text_file_description

--WHERE source_id = @source_id
GO


