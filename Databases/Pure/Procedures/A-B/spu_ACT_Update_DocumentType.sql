SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_DocumentType'
GO


CREATE PROCEDURE spu_ACT_Update_DocumentType
    @documenttype_id smallint,
    @doctypegroup_id smallint,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10)
AS


BEGIN
UPDATE DocumentType
    SET
    doctypegroup_id=@doctypegroup_id,
    caption_id=@caption_id,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    description=@description,
    code=@code
WHERE documenttype_id = @documenttype_id
END
GO


