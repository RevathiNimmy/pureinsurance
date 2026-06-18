SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Document_Type_add'
GO

CREATE PROCEDURE spu_Document_Type_add
    @document_type_id smallint,
    @caption_id smallint,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @is_editable_after_merging tinyint
AS
/* Insert the values */
INSERT INTO Document_Type
( document_type_id, caption_id, code, description, is_deleted, effective_date, is_editable_after_merging )
VALUES
( @document_type_id, @caption_id, @code, @description, @is_deleted, @effective_date, @is_editable_after_merging )
GO

