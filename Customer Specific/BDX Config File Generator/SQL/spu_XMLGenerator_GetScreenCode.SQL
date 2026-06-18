/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetScreenCode]    Script Date: 18-05-2016 5:23:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetScreenCode]
( 
	@ObjectName Varchar(100),
	@ScreenId integer
)
AS
BEGIN
DECLARE @GisObjectID int
DECLARE @GisObjectParentId int
DECLARE @GisDataModelId int
select @GisDataModelId=gis_data_model_id  from GIS_Screen where gis_screen_id =@ScreenId
SELECT @GisObjectID=GIS_Object_id from GIS_Object where object_name =@ObjectName AND gis_data_model_id =@GisDataModelId 
SELECT @GisObjectParentId=Gobj.parent_object_id   FROM GIS_Object GObj where GObj.gis_object_id =@GisObjectID
SELECT gobj.object_name,Gobj.parent_object_id   FROM GIS_Object GObj where GObj.gis_object_id =@GisObjectParentId
END