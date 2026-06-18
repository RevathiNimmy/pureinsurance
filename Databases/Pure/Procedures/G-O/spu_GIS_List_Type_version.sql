SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_version'
GO

CREATE PROCEDURE spu_GIS_List_Type_version
    @id	int
AS
BEGIN
SELECT DISTINCT version from GIS_List_Type_Usage where gis_list_type_id=@id order by version
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
