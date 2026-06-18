SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_peril_types'
GO


CREATE PROCEDURE spu_get_peril_types
AS


SELECT peril_type_id,Code, Description, gis_Screen_id
FROM peril_type
where is_deleted = 0
ORDER BY Code
GO


