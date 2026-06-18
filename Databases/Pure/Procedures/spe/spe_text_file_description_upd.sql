SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_description_upd'
GO

CREATE PROCEDURE spe_text_file_description_upd
    @source_id int,
    @entity_type_id int,
    @slot_number int,
    @description varchar(255)
AS
BEGIN
UPDATE text_file_description
    SET
    description=@description
WHERE source_id = @source_id AND entity_type_id = @entity_type_id AND slot_number = @slot_number
END

GO

