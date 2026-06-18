
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_distinct_loop3_values'
GO

CREATE PROCEDURE spu_get_distinct_loop3_values
	@Loop1 AS VARCHAR(255),
	@Loop2 AS VARCHAR(255)
AS        
        
BEGIN 

	SELECT DISTINCT loop3, Table_name 
	FROM wp_fields
	WHERE loop1 = @Loop1	
	AND loop2 = @Loop2 
	AND ISNULL(Table_Name,'') <> ''
	AND data_model IS NULL
	AND loop3 IS NOT NULL

END  
GO