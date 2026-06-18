SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_add'
GO

CREATE PROCEDURE spu_GIS_List_Type_add
    @code char(20),
    @description varchar(255)
AS
BEGIN
INSERT gis_list_type (code,description,is_deleted) VALUES (@code,@description,0)
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
