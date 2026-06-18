SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Parent_Node_ID_For_Tax'
GO

CREATE PROCEDURE spu_Get_Parent_Node_ID_For_Tax    
AS

SELECT	node_id 
FROM	StructureTree 
WHERE	element_id IN(SELECT element_id FROM element WHERE RTRIM(UPPER(element_name))='TAX')

GO




