SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_description_add'
GO

CREATE PROCEDURE spe_text_file_description_add
    @source_id int,
    @entity_type_id int,
    @slot_number int,
    @description varchar(255)
AS
BEGIN
INSERT INTO text_file_description (
    source_id ,
    entity_type_id ,
    slot_number ,
    description )
VALUES (
    @source_id,
    @entity_type_id,
    @slot_number,
    @description)
END

GO

