SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_AddNewPropertyHH'
GO


CREATE PROCEDURE spu_AddNewPropertyHH
    @property_name varchar(70),
    @object_name varchar(70),
    @sql_data_type varchar(70),
    @gis_data_type int
AS


DECLARE @property_id int
DECLARE @object_id int
DECLARE @SQL       varchar(255)

SELECT @property_id = MAX(gis_property_id) + 1 FROM gis_property WHERE gis_property_id < 25000
SELECT @object_id = gis_object_id FROM gis_object WHERE object_name = @object_name AND gis_data_model_id = 2

IF NOT EXISTS (SELECT * FROM gis_property WHERE property_name = @property_name)
BEGIN
   INSERT INTO gis_property
       (gis_property_id,
       gis_object_id,
       property_name,
       column_name,
       data_type,
       is_input_property,
       is_identifying_property,
       is_primary_key)
   VALUES
   (@property_id,@object_id,@property_name,@property_name,@gis_data_type,0,0,0)
END

SELECT @SQL = "ALTER TABLE " + @object_name + " ADD " + @property_name + " " +  @sql_data_type + " NULL"

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id = object_id(@object_name)
    AND name = @property_name)
BEGIN
    EXEC(@SQL)
END
GO


