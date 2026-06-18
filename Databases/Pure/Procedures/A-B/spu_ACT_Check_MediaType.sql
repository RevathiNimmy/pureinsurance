SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_MediaType'
GO


CREATE PROCEDURE spu_ACT_Check_MediaType
    @mediatype_id int OUTPUT
AS


BEGIN
    SELECT @mediatype_id = mediatype_id
    FROM MediaType
    WHERE mediatype_id = @mediatype_id
END
BEGIN
IF @mediatype_id = NULL
    SELECT @mediatype_id = -1
END
GO


