SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_Is_Unique'
GO

CREATE PROCEDURE spu_GIS_List_Type_Is_Unique
    @code char(20),
    @description varchar(255)
AS
BEGIN
SELECT * FROM gis_list_type WHERE code= @code OR description = @description
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
