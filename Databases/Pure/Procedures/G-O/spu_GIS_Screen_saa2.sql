SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Screen_saa2'
GO

CREATE PROCEDURE spu_GIS_Screen_saa2

AS

SELECT
    gis_screen_id,
    code,
    description,
    effective_date
FROM GIS_Screen
WHERE parent_id IS NULL
AND is_deleted = 0
AND (
        product_option IS NULL 
        OR 
        product_option IN 
            (
                SELECT 
                    option_number 
                FROM hidden_options 
                WHERE value = '1'
            )
    )
ORDER BY code

GO


