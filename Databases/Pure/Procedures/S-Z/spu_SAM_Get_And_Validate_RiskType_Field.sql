SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_And_Validate_RiskType_Field'
GO


CREATE PROCEDURE spu_SAM_Get_And_Validate_RiskType_Field
  
@table_name varchar(255),  
@field_to_return_name varchar(255),  
@field_to_validate_name varchar(255),  
@field_to_validate_value varchar(255)  ,
@Risk_Type_id int  =0

AS  
  
 DECLARE @sSQL nvarchar(4000)  
  
 SET @sSQL =''  
 SET @sSQL =@sSQL +  'SELECT ' + @field_to_return_name + ' FROM ' + @table_name  
 SET @sSQL = @sSQL +  ' A JOIN gis_data_model on gis_data_model.gis_data_model_id= A.gis_data_model_id  '
 SET @sSQL = @sSQL +  ' JOIN  GIS_Screen on GIS_Screen.gis_data_model_id=A.gis_data_model_id' 
 SET @sSQL = @sSQL + ' join Risk_Type on Risk_Type.gis_screen_id=GIS_Screen.gis_screen_id'
 SET @sSQL = @sSQL + ' WHERE ' + @field_to_validate_name + ' = '  
 SET @sSQL = @sSQL + '''' + replace(@field_to_validate_value,'''','''''') + ''''  

  SET @sSQL = @sSQL + ' and risk_type_id=  ' + CONVERT(varchar(10),@Risk_Type_id)    
  

 EXEC sp_executesql @sSQL

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
