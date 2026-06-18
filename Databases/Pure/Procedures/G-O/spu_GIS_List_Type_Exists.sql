SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_Exists'
GO

CREATE PROCEDURE spu_GIS_List_Type_Exists
    @table varchar(50)
AS
BEGIN
SELECT * FROM Information_schema.tables where table_name=@table
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
