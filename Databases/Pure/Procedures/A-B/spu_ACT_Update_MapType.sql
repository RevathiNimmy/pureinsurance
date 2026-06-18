SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_MapType'
GO


CREATE PROCEDURE spu_ACT_Update_MapType
    @maptype_id smallint,
    @description varchar(255),
    @code char(10),
    @is_deleted bit,
    @effective_date datetime,
    @caption_id int
AS


BEGIN
UPDATE MapType
    SET
    description=@description,
    code=@code,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    caption_id=@caption_id
WHERE maptype_id = @maptype_id
END
GO


