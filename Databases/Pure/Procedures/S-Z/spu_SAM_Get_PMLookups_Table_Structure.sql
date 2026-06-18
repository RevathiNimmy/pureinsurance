
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_PMLookups_Table_Structure'
GO

CREATE PROCEDURE spu_SAM_Get_PMLookups_Table_Structure     
   @Tablename  VARCHAR(255)
AS
BEGIN
SELECT column_name,data_type,  is_nullable,character_maximum_length FROM
information_schema.COLUMNS WHERE table_name = @Tablename
ORDER BY ordinal_position
END
GO
  
