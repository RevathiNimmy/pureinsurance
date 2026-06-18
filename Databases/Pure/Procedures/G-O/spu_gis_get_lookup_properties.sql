SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_gis_get_lookup_properties'
GO


CREATE PROCEDURE spu_gis_get_lookup_properties
    @gis_data_model_code CHAR(10)
    
AS
BEGIN
	CREATE TABLE #temp_properties (
	    temp_object_name VARCHAR(70),
	    temp_property_name VARCHAR(70),
	    TEMP_Specials_Type_Reference VARCHAR(50),
	    temp_specials_type INTEGER
	    )
	    
	INSERT INTO #temp_properties 
		SELECT o.object_name, p.property_name, p.Specials_Type_Reference, p.specials_type
			FROM gis_object o
			INNER JOIN gis_property p ON p.gis_object_id = o.gis_object_id
			INNER JOIN gis_data_model d ON d.gis_data_model_id = o.gis_data_model_id
			WHERE (p.Specials_Type_Reference is NOT NULL AND p.Specials_Type_Reference <> '')
			AND d.code = @gis_data_model_code
			
	INSERT INTO #temp_properties VALUES ('STS_EDI_COMMON', 'CLI_MARITAL', 5, 1)
	INSERT INTO #temp_properties VALUES ('STS_EDI_COMMON', 'CLI_SEX', 6, 1)
	INSERT INTO #temp_properties VALUES ('STS_EDI_COMMON', 'CLI_TITLE', 53, 1)
	
	SELECT * FROM #temp_properties 
	
	DROP TABLE #temp_properties 

END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

