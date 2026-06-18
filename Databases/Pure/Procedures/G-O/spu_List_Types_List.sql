EXECUTE DDLDropProcedure 'spu_List_Types_List'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_List_Types_List AS

SELECT gis_list_type_id, description
FROM gis_list_type
WHERE is_deleted = 0
ORDER BY description
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
