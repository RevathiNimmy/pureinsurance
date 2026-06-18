EXEC DDLDROPPROCEDURE
  'spu_SG_GetListCode'

GO

SET QUOTED_IDENTIFIER ON

GO

SET ANSI_NULLS ON

GO

CREATE PROCEDURE spu_sg_getlistcode (@sGis_data_model_code CHAR(10),
                                     @sObject_name         VARCHAR(70),
                                     @sProperty_name       VARCHAR(70),
                                     @sCode                CHAR(10))
AS
  -- Returns the gis_user_def_detail_id for the selected OBJECT_NAME, PROPERTY_NAME and CODE values  
  BEGIN
      DECLARE @nGis_data_model_id INT
      DECLARE @nSpecials_type INT
      DECLARE @sSpecials_type_reference VARCHAR(100)

      -- Get the gis_data_model_id for the selected data model code  
      SELECT @nGis_data_model_id = gis_data_model_id
      FROM   gis_data_model
      WHERE  code = @sGis_data_model_code

      SELECT @nSpecials_type = specials_type,
             @sSpecials_type_reference = RTRIM(LTRIM(specials_type_reference))
      FROM   gis_property gp
             JOIN gis_object gob
               ON gob.gis_object_id = gp.gis_object_id
      WHERE  gob.[object_name] = @sObject_name
             AND gp.property_name = @sProperty_name

      IF ( @nSpecials_type = 6 )
        BEGIN
            SELECT gis_user_def_detail_id,
                   gis_user_def_header.code
            FROM   gis_object
                   JOIN gis_property
                     ON gis_object.gis_object_id = gis_property.gis_object_id
                   JOIN gis_user_def_header
                     ON gis_property.specials_type_reference = gis_user_def_header.gis_user_def_header_id
                   JOIN gis_user_def_detail
                     ON gis_user_def_header.gis_user_def_header_id = gis_user_def_detail.gis_user_def_header_id
            WHERE  gis_data_model_id = @nGis_data_model_id
                   AND [object_name] = @sObject_name
                   AND property_name = @sProperty_name
                   AND gis_property.specials_type = 6
                   AND gis_user_def_detail.code = @sCode
                   AND ( gis_property.is_deleted <> 1
                          OR gis_property.is_deleted IS NULL )
        END

      --udl
      IF ( @nSpecials_type = 2 )
        BEGIN
            DECLARE @sql NVARCHAR(4000)

            SET @sql = 'select ' + @sSpecials_type_reference
                       + '_id as gis_user_def_detail_id, '''' as code from '
                       + @sSpecials_type_reference
                       + ' where (is_deleted <> 1 or is_deleted is null) and code ='''
                       + @sCode + ''''

            EXEC SP_EXECUTESQL
              @sql
        END
  END

GO

SET QUOTED_IDENTIFIER OFF

GO

SET ANSI_NULLS ON

GO 
