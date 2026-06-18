SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_CheckIdentityExists'
GO
CREATE PROCEDURE spu_CheckIdentityExists
@tablename SYSNAME
AS
IF EXISTS (select 1 from sys.columns c where c.object_id = object_id(@tablename) and c.is_identity =1)
BEGIN
	SELECT 1
END
ELSE
BEGIN
	SELECT 0
END
