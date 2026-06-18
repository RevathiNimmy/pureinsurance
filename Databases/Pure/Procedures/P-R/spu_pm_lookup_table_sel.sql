SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pm_lookup_table_sel'
GO


CREATE PROCEDURE spu_pm_lookup_table_sel
    @pmproduct_id INT
AS

SELECT 
    lookup_table_name,
    edit_privilege_level
FROM PMProduct_Lookup
WHERE pmproduct_id = ISNULL(@pmproduct_id, pmproduct_id)
AND is_generic_maintenance = 1
ORDER BY lookup_table_name

GO


