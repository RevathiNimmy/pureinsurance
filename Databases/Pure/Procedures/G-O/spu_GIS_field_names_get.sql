SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_Field_Names_Get'
GO

CREATE PROCEDURE spu_GIS_Field_Names_Get
    @table varchar(50)
AS

IF Exists(Select 1 From gis_list_type where 'UDL_'+code=@table)
BEGIN
	Select c.column_name FROM INFORMATION_SCHEMA.TABLES t,inFORMATION_SCHEMA.COLUMNS c 
	Where t.table_name = c.table_name and t.table_name=@table
	AND c.column_name not like 'udl_version'
END
ELSE
BEGIN
	Select c.column_name FROM INFORMATION_SCHEMA.TABLES t,inFORMATION_SCHEMA.COLUMNS c 
	Where t.table_name = c.table_name and t.table_name=@table
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

