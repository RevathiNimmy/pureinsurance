SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Item_upd'
GO

CREATE PROCEDURE spu_GIS_List_Item_upd
    @newcode VARCHAR(100),
    @listtype VARCHAR(100),
    @lookup VARCHAR(100)
AS
BEGIN

DECLARE @stmt NVARCHAR(400)

SELECT @stmt = 'UPDATE gis_list_items SET Code=' + @newcode + ' From gis_list_items gli INNER JOIN UDL_' + @listtype + ' udl ON gli.code=udl.code WHERE udl.udl_' + @listtype + '_id =' + @lookup

EXECUTE sp_executeSQL @stmt

END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO