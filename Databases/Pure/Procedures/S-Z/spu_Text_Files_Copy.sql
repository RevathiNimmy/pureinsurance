SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Text_Files_Copy'
GO

CREATE PROCEDURE spu_Text_Files_Copy
    @oldinsurance_file_cnt INT,
    @newinsurance_file_cnt INT
AS
BEGIN

DECLARE @slot_number INT
DECLARE @file_number INT

DECLARE c_cursor CURSOR FORWARD_ONLY FOR
SELECT slot_number, file_number
FROM text_file 
WHERE entity_type_id = 2
AND entity_cnt = @oldinsurance_file_cnt

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO @slot_number, @file_number 

WHILE @@FETCH_STATUS = 0
BEGIN

	EXEC spu_text_file_add 2, @newinsurance_file_cnt, @slot_number, @file_number OUTPUT

	FETCH NEXT FROM c_cursor INTO @slot_number, @file_number 

END

CLOSE c_cursor
DEALLOCATE c_cursor

END
GO