SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'Spu_Get_Gis_Property_Column_Name'
GO



CREATE PROCEDURE Spu_Get_Gis_Property_Column_Name
	@gis_data_model_code 	VARCHAR(50),
	@gis_object_name 	VARCHAR(50),
	@property_name 		VARCHAR(50),
	@table_name		VARCHAR(50) OUTPUT,
	@Column_Name		VARCHAR(50) OUTPUT
AS

       SELECT   @table_name=table_name,
   	        @Column_Name=gp.column_name
       FROM     gis_object go
   	        INNER JOIN gis_data_model gdm
    		      ON gdm.gis_data_model_id = go.gis_data_model_id
                INNER JOIN gis_property gp
    		     ON gp.gis_object_id = go.gis_object_id
       WHERE    UPPER(gp.property_name) = @property_name
       AND      UPPER(go.object_name) = @gis_object_name
       AND      gdm.code = @gis_data_model_code

GO
