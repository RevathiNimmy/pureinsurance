SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Text_Files_Sel_All'
GO

CREATE PROCEDURE spu_Text_Files_Sel_All
	@entity_type_id INT,
    @entity_cnt INT
AS
BEGIN

SELECT slot_number, file_number
FROM text_file 
WHERE entity_type_id = @entity_type_id 
AND entity_cnt = @entity_cnt

END
GO