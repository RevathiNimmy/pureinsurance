SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Item_get'
GO

CREATE PROCEDURE spu_GIS_List_Item_get
    @listtype VARCHAR(100),
    @lookup VARCHAR(100)
AS
BEGIN

DECLARE @stmt NVARCHAR(300)

SELECT @stmt = 'SELECT code from UDL_' + @ListType + ' WHERE UDL_' + @ListType + '_id=' +  @Lookup + ''

EXECUTE  sp_executeSQL @stmt

END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO