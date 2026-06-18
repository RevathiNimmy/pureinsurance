SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_Find_Mapping_Add'
GO

CREATE PROCEDURE spu_GIS_Find_Mapping_Add  
 @NEW_FindControl_Id int OUTPUT,  
 @FindControl_ID int,  
 @ControlIndex int,  
 @ViewFieldName varchar(128),  
 @ControlType int,  
 @Fuzzy bit,  
 @ViewName varchar(128),  
 @gis_object_id int,  
 @gis_property_id int,  
 @object_name varchar(70),  
 @property_name varchar(70),  
 @grid_caption varchar(255),  
 @grid_position int,  
 @grid_width int  
  
AS  
  
BEGIN  
  
 -- get the new find control id  
 IF ISNULL(@FindControl_ID,-1) =-1  
  BEGIN  
   SELECT @NEW_FindControl_ID = ISNULL(MAX(FindControl_ID),0) + 1 from GIS_Find_mapping  
   SELECT @FindControl_ID = @NEW_FindControl_ID  
  END  
  
 -- create the gis find mapping entry  
 INSERT INTO gis_find_mapping (  
  FindControl_ID,  
  ControlIndex,  
  ViewFieldName,  
  ControlType,  
  Fuzzy,  
  ViewName,  
  gis_object_id,  
  gis_property_id,  
  object_name,  
  property_name,  
  grid_caption,  
  grid_position,  
  grid_width)  
 VALUES (  
  @FindControl_ID,  
  @ControlIndex,  
  @ViewFieldName,  
  @ControlType,  
  @Fuzzy,  
  @ViewName,  
  @gis_object_id,  
  @gis_property_id,  
  @object_name,  
  @property_name,  
  @grid_caption,  
  @grid_position,  
  @grid_width)  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
