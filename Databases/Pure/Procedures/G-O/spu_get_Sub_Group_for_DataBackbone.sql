
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Sub_Group_for_DataBackbone'
GO


/*************************************************************************/  
/*Get sub_group fields for Data Backbone structure*/
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_Sub_Group_for_DataBackbone
@DataModelCode AS VARCHAR(50)
AS        
        
BEGIN  
	SELECT DISTINCT 	
	sub_group	
	FROM wp_fields 
	WHERE data_model=@DataModelCode	
	order by sub_group
END  
GO