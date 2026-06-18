SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_text_file_description_sel'
GO


CREATE PROCEDURE spu_text_file_description_sel
    @source_id int,
    @language_id int
AS


SELECT  tfd.entity_type_id,
    c.caption,
    tfd.slot_number,
    tfd.description,
    tfd.source_id
FROM    text_file_description tfd,
    entity_type et,
    PMCaption c
WHERE   tfd.entity_type_id = et.entity_type_id
AND et.is_deleted = 0
AND et.caption_id = c.caption_id
--AND   tfd.source_id = @source_id
AND c.language_id = @language_id
GO


