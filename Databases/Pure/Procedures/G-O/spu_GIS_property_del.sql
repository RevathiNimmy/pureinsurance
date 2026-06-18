/**********************************************************************************************/
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
 
ddldropprocedure 'spu_GIS_property_del'
GO

CREATE PROCEDURE spu_GIS_property_del
   @gis_property_id int,
   @gis_object_id int

as

DELETE FROM gis_screen_detail  WHERE gis_property_id  = @gis_property_id and gis_object_id=@gis_object_id

DELETE FROM gis_property WHERE gis_property_id  = @gis_property_id and gis_object_id=@gis_object_id
GO
/**********************************************************************************************/
