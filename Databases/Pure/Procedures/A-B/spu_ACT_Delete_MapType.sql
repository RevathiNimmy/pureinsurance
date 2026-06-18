SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_MapType'
GO


CREATE PROCEDURE spu_ACT_Delete_MapType
    @maptype_id smallint
AS


DELETE FROM MapType
WHERE maptype_id = @maptype_id
GO


