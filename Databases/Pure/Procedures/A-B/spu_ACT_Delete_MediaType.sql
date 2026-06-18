SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_MediaType'
GO


CREATE PROCEDURE spu_ACT_Delete_MediaType
    @mediatype_id int
AS


DELETE FROM MediaType
WHERE mediatype_id = @mediatype_id
GO


