
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Core_Fields_for_DataBackbone'
GO


/*************************************************************************/  
/*Get Core fields for Data Backbone structure*/
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_Core_Fields_for_DataBackbone

AS        
        
BEGIN 

	SELECT DISTINCT Table_name 
	FROM wp_fields
	WHERE loop1 IS NULL 
	AND loop2 IS NULL
	AND loop3 IS NULL
	AND ISNULL(Table_Name,'') <> ''
	AND data_model IS NULL

END  
GO