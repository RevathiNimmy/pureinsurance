SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_BackdatedMTA_DaysAllowed_sel'
GO

CREATE PROCEDURE spu_GIS_Scheme_BackdatedMTA_DaysAllowed_sel
    @GIS_Scheme_BackdatedMTA_DaysAllowed_id int
AS
SELECT  GIS_Scheme_BackdatedMTA_DaysAllowed_id,
    	caption_id,
    	is_deleted,
    	effective_date,
    	code,
    	description,
    	no_of_days
FROM 	GIS_Scheme_BackdatedMTA_DaysAllowed
WHERE   GIS_Scheme_BackdatedMTA_DaysAllowed_id = @GIS_Scheme_BackdatedMTA_DaysAllowed_id
 
GO