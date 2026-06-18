SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spu_Rule_type_select_filtered'

GO

CREATE PROCEDURE spu_Rule_type_select_filtered 
AS	
BEGIN
	SELECT description,risk_type_rule_set_type_id 
	FROM risk_type_rule_set_type 
	WHERE is_deleted = 0 AND CODE IN('SCRIPT','COMPILED') 
	ORDER BY description
END

GO 
