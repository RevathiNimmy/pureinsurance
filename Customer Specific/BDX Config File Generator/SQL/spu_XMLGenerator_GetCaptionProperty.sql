/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetCaptionProperty]    Script Date: 18-05-2016 5:20:43 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetCaptionProperty]
( 
	@PropertyName varchar(100)
)
AS
BEGIN
	declare @ItemTop as int
	declare @ItemHeight as int
	declare @ItemWidth as int
	declare @ParentId as int
	declare @GisScreenId as int
	select @ItemTop=item_top, @ItemHeight=item_height,@ItemWidth=item_width,@ParentId=parent_id,@GisScreenId=gis_screen_id 
	from GIS_screen_detail where gis_property_id=(select gis_property_id from gis_property where property_name=@PropertyName)
	select caption from gis_screen_detail  
	where parent_id=@ParentId and item_top =@ItemTop and item_height =@ItemHeight and item_width = @ItemWidth  and help_text is not null and gis_screen_id =@GisScreenId and gis_property_id =-1

End