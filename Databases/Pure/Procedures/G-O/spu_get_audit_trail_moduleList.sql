SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_audit_trail_moduleList'
GO

CREATE PROCEDURE spu_get_audit_trail_moduleList
AS

SELECT Modules_id,ModuleName from Audit_Trail_Modules ORDER BY 2 

GO