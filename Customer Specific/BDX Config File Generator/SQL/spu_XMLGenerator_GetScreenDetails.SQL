/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetScreenDetails]    Script Date: 18-05-2016 5:24:05 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetScreenDetails]
( 
	@RiskTypeId integer
)
AS
BEGIN
	if @RiskTypeId <>0
	BEGIN
		Declare @Gis_Screen_Id Integer 
		select @Gis_Screen_Id=gis_screen_id  from risk_type  where risk_type_id =@RiskTypeId
		select gsd .caption  ,gobj.object_name ,gp.property_name,gp.data_type ,gsd.parent_id ,gsd.gis_screen_id,GP.Specials_Type from GIS_Property GP
		join gis_screen_detail gsd on gsd.gis_property_id =gp.gis_property_id 
		join gis_object gobj on gobj.gis_object_id =gp.gis_object_id  
		where GP.gis_object_id in (select distinct gis_object_id  from gis_screen_detail 
		where gis_screen_id =@Gis_Screen_Id and gis_object_id is not null) 
		--and GP.property_name not like '%ID%'
		 and gp.is_deleted =0
	END
ELSE
	BEGIN
		select gsd .caption  ,gobj.object_name ,gp.property_name,gp.data_type ,gsd.parent_id ,gsd.gis_screen_id,GP.Specials_Type from GIS_Property GP
		join gis_screen_detail gsd on gsd.gis_property_id =gp.gis_property_id 
		join gis_object gobj on gobj.gis_object_id =gp.gis_object_id  
		where GP.gis_object_id in (select distinct gis_object_id  from gis_screen_detail 
		where gis_screen_id IN(select gis_screen_id  from risk_type  where Is_deleted =0) and gis_object_id is not null) 
		--and GP.property_name not like '%ID%' 
		and gp.is_deleted =0
	END
END
