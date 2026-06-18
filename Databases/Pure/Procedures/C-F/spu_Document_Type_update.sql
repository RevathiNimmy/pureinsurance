SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Document_Type_update'
GO

CREATE PROCEDURE spu_Document_Type_update
    @document_type_id smallint,
    @caption_id smallint,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @is_editable_after_merging tinyint
AS
/* Update the values */
UPDATE  Document_Type
SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    is_editable_after_merging = @is_editable_after_merging
WHERE   document_type_id = @document_type_id
GO

