SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_text_file_add'
GO


CREATE PROCEDURE spu_text_file_add
    @entity_type_id int,
    @entity_cnt int,
    @slot_number int,
    @file_number int OUTPUT
AS


BEGIN
SELECT @file_number = next_file_number
FROM    text_file_number
WHERE   entity_type_id = @entity_type_id
IF @file_number IS NULL
BEGIN
    SELECT  @file_number = 1
    INSERT INTO text_file_number (
        entity_type_id,
        next_file_number)
    VALUES (@entity_type_id,
        2)
END
ELSE
BEGIN
    UPDATE  text_file_number
    SET next_file_number = next_file_number + 1
    WHERE   entity_type_id = @entity_type_id
END
INSERT INTO text_file (
    entity_type_id ,
    entity_cnt ,
    slot_number ,
    file_number )
VALUES (
    @entity_type_id,
    @entity_cnt,
    @slot_number,
    @file_number)
END
GO


