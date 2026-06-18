SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_get'
GO

CREATE PROCEDURE spu_GIS_List_Type_get
AS
BEGIN
SELECT [gis_list_type_id], [code], [description], [is_deleted] FROM [gis_list_type] order by description
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
