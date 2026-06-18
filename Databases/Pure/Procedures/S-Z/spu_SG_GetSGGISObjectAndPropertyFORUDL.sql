EXEC DDLDROPPROCEDURE
  'spu_SG_GetSGGISObjectAndPropertyFORUDL'

GO

SET QUOTED_IDENTIFIER ON

GO

SET ANSI_NULLS ON

GO

CREATE PROCEDURE spu_sg_getsggisobjectandpropertyforudl @sData_model_code CHAR(10)
AS
  BEGIN
      SET NOCOUNT ON

      SELECT GO.GIS_OBJECT_ID,
             GO.OBJECT_NAME,
             GP.GIS_PROPERTY_ID,
             GP.column_name,
             GP.PROPERTY_NAME,
             GP.specials_type_reference
      FROM   GIS_OBJECT GO
             JOIN GIS_PROPERTY GP
               ON GO.GIS_OBJECT_ID = GP.GIS_OBJECT_ID
                  AND GP.specials_type = 2
                  AND gp.specials_type_reference LIKE 'UDL_%'
      WHERE  ( GP.IS_DELETED <> 1
                OR GP.IS_DELETED IS NULL )
             AND GO.GIS_DATA_MODEL_ID IN (SELECT DISTINCT ( GIS_DATA_MODEL_ID )
                                          FROM   GIS_DATA_MODEL
                                          WHERE  CODE = @sData_model_code)
      ORDER  BY GO.OBJECT_NAME,
                GP.PROPERTY_NAME
  END

GO

SET QUOTED_IDENTIFIER OFF

GO

SET ANSI_NULLS ON

GO 
