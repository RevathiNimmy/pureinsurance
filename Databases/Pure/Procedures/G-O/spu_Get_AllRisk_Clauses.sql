SET QUOTED_IDENTIFIER OFF 
GO
EXECUTE DDLDropProcedure 'spu_Get_AllRisk_Clauses'
GO

CREATE PROCEDURE spu_Get_AllRisk_Clauses  
AS  
--Get the List of all Clauses
SELECT DISTINCT  wf.field_name AS column_name
	FROM wp_fields wf  
	WHERE wf.specials_type = 5 