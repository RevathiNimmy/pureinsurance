SET QUOTED_IDENTIFIER OFF

GO

SET ANSI_NULLS ON

GO

EXECUTE DDLDROPPROCEDURE
  'spu_GIS_GetObjectIdByName'

GO

CREATE PROCEDURE spu_GIS_GetObjectIdByName @sObjectName VARCHAR(70)
AS
    SELECT gis_object_id
    FROM   GIS_Object
    WHERE  object_name = @sObjectName

GO 
