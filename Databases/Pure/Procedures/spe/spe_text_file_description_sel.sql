SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_text_file_description_sel'
GO

CREATE PROCEDURE spe_text_file_description_sel
    @source_id int,
    @entity_type_id int,
    @slot_number int
AS
SELECT
    source_id,
    entity_type_id,
    slot_number,
    description
 FROM text_file_description
WHERE source_id = @source_id AND entity_type_id = @entity_type_id AND slot_number = @slot_number

GO

