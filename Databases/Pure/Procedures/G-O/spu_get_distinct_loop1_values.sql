
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_distinct_loop1_values'
GO

CREATE PROCEDURE spu_get_distinct_loop1_values

AS        
        
BEGIN 

	SELECT DISTINCT loop1 
	FROM wp_fields
	WHERE loop1 IS NOT NULL 	
	AND ISNULL(Table_Name,'') <> ''
	AND data_model IS NULL

END  
GO