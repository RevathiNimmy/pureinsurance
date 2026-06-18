
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_distinct_loop2_values'
GO

CREATE PROCEDURE spu_get_distinct_loop2_values
	@Loop1 AS VARCHAR(255)
AS        
        
BEGIN 

	SELECT DISTINCT loop2, Table_name 
	FROM wp_fields
	WHERE loop1 = @Loop1	
	AND ISNULL(Table_Name,'') <> ''
	AND data_model IS NULL
	AND loop2 IS NOT NULL

END  
GO