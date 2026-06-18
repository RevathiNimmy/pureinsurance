SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_MapType'
GO


CREATE PROCEDURE spu_ACT_Check_MapType
    @maptype_id smallint OUTPUT
AS


BEGIN
    SELECT @maptype_id = maptype_id
    FROM MapType
    WHERE maptype_id = @maptype_id
END
BEGIN
IF @maptype_id = NULL
    SELECT @maptype_id = -1
END
GO


